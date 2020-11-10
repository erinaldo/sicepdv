using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Xml.Linq;
using System.Configuration;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Data.EntityClient;
using System.Data.SqlClient;

namespace SICEpdv
{
    class FuncoesNFC
    {
        public int idCliente { get; set; }
        public decimal descontoVenda { get; set; }
        public decimal encargosVenda { get; set; }
        public string serie { get; set; }
        public int numeroNFCe { get; set; }
        public IEnumerable<venda> itens { get; set; }
        public IEnumerable<caixa> pagamentos { get; set; }
        public static StringBuilder conteudoImpressao = new StringBuilder();
        private StringBuilder cabecalho = new StringBuilder();
        private StringBuilder rodaPe = new StringBuilder();
        private StringBuilder impressao = new StringBuilder();

        public bool FinalizarVenda()
        {

            int iRetorno = 0;
            int iStatus = 0;
            //Abrindo Venda
            int codigoMunicipio = this.codigoMunicipio(Venda.dadosConsumidor.endCidade, this.codigoEstado(Venda.dadosConsumidor.endEstado));

            //iRetorno = DARUMA_FW.aCFAbrir_NFCe_Daruma(Venda.dadosConsumidor.cpfCnpjConsumidor.Trim(), Venda.dadosConsumidor.nomeConsumidor.Trim(), Venda.dadosConsumidor.endConsumidor.Trim(), Venda.dadosConsumidor.endNumero.Trim(), Venda.dadosConsumidor.endBairro.Trim(),codigoMunicipio.ToString() , Venda.dadosConsumidor.endCidade, Venda.dadosConsumidor.endEstado, Venda.dadosConsumidor.endCEP);
            if (DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() < 1 || DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() == 5)
                iRetorno = DARUMA_FW.aCFAbrirNumSerie_NFCe_Daruma(numeroNFCe.ToString(), serie, Venda.dadosConsumidor.cpfCnpjConsumidor.Trim(), Venda.dadosConsumidor.nomeConsumidor.Trim(), Venda.dadosConsumidor.endConsumidor.Trim(), Venda.dadosConsumidor.endNumero.Trim(), Venda.dadosConsumidor.endBairro.Trim(), codigoMunicipio.ToString(), Venda.dadosConsumidor.endCidade, Venda.dadosConsumidor.endEstado, Venda.dadosConsumidor.endCEP);

            iStatus = DARUMA_FW.rCFVerificarStatus_NFCe_Daruma();
            
            if (iRetorno != 1)
            {
                iRetorno = DARUMA_FW.rCFVerificarStatus_NFCe_Daruma();

                /*if (iRetorno != 1)
                    throw new Exception(DARUMA_FW.TrataRetorno(iRetorno));*/
            }

                if (DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() < 2)
                {
                //Imprimindo os Itens
                foreach (var item in itens)
                {

                    decimal desconto = item.Descontoperc;
                    string tipoDesconto = "D%";
                    if (item.descontovalor > 0)
                    {
                        desconto = item.descontovalor;
                        tipoDesconto = "D$";
                    }

                    if (item.acrescimototalitem > 0)
                    {
                        tipoDesconto = "A$";
                        desconto = Convert.ToDecimal(string.Format("{0:N2}", (item.acrescimototalitem) * -1));
                    }

                    // iRetorno = DARUMA_FW.aCFVender_NFCe_Daruma("F1", "1,00", "0,10", tipoDesconto, "0,00", item.codigo,"UN", item.produto);
                    iRetorno = DARUMA_FW.aCFVender_NFCe_Daruma(Venda.TributacaoCupom(item.tributacao, item.icms), string.Format("{0:N3}", item.quantidade), string.Format("{0:N2}", item.precooriginal), tipoDesconto, desconto.ToString(), item.codigo.Trim(), "UN", item.produto.Trim());

                    if (iRetorno != 1)
                    {
                        throw new Exception("Não foi possível vender o item: " + item.codigo + " " + item.produto + "  NFC Retorno: " + iRetorno.ToString());
                    }
                }
            }
            //Iniciando Finalização

            decimal valorAD = descontoVenda;

            string tipoAD = "D";
            if (encargosVenda > 0)
            {
                tipoAD = "A";
                valorAD = encargosVenda;
            }
            if (DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() < 3)
                iRetorno = DARUMA_FW.aCFTotalizar_NFCe_Daruma(tipoAD, string.Format("{0:N2}", valorAD).ToString());


            //Iniciando Pagamento
            foreach (var itemPag in pagamentos)
            {

                string descricaoPagamento = " ";

                switch (itemPag.tipopagamento)
                {
                    case "DH":
                        descricaoPagamento = ConfiguracoesECF.DH;
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

                
                if (DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() < 4 )
                    iRetorno = DARUMA_FW.aCFEfetuarPagamento_NFCe_Daruma(descricaoPagamento, itemPag.valor.ToString());
            }

         
            if (DARUMA_FW.rCFVerificarStatus_NFCe_Daruma() < 5)
                iRetorno = DARUMA_FW.tCFEncerrar_NFCe_Daruma(Configuracoes.mensagemRodapeCupom);


            if (iRetorno == 0)
            {
                if (ConfiguracoesECF.NFCmodImpressao == "M")
                    throw new Exception(DARUMA_FW.TrataRetorno(iRetorno));

                int retorno = DARUMA_FW.rCFVerificarStatus_NFCe_Daruma();

                if (retorno != 5 && retorno != 0)
                    return false;
                else
                    return true;

            }
            if (iRetorno != 1)
            {
                if (iRetorno == -6)
                {
                    MessageBox.Show("Numero da NFCe.: " + numeroNFCe + " já foi utilizado pelo distema da SEFAZ! \n O Sistema reenviará a nota novamente com uma nova Numeração", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Venda.ObterSequencialNFC();
                    this.GuardarSequenciaNFC(ConfiguracoesECF.NFCSequencia.ToString());
                    this.numeroNFCe = ConfiguracoesECF.NFCSequencia;
                    this.FinalizarVenda();
                     
                }
                else
                {
                    
                    // retorno == -135 colocar um comando para excluir o GNE_frameworck 

                    if (iRetorno < -129)
                        if (File.Exists("GNE_Framework.xml"))
                            File.Delete("GNE_Framework.xml");
                        // retorno == -135 colocar um comando para excluir o GNE_frameworck 


                    this.numeroNFCe = ConfiguracoesECF.NFCSequencia;
                    if (this.FinalizarVenda() == true)
                        return true;
                    

                    /*StringBuilder StrCodAvisoErro = new StringBuilder();
                    StrCodAvisoErro.Length = 4;
                    StringBuilder StrMsgAvisoErro = new StringBuilder();
                    StrMsgAvisoErro.Length = 600;
                    DARUMA_FW.rAvisoErro_NFCe_Daruma(StrCodAvisoErro, StrMsgAvisoErro);
                    MessageBox.Show(StrCodAvisoErro.ToString() + " - " + StrMsgAvisoErro.ToString());*/

                    //- 5 o web serve da daruma retorna Erro desconhecido;
                    //return true;
                    throw new Exception(DARUMA_FW.TrataRetorno(iRetorno));
                }
            }
            return true;
        }

        public string informacaoUltimaNFCe(string indice)
        {
            try
            {
                StringBuilder StrRetorno = new StringBuilder();
                StrRetorno.Length = 44;
                int iRetorno;
                iRetorno = DARUMA_FW.rInfoEstendida_NFCe_Daruma(indice, StrRetorno);
                return StrRetorno.ToString();

            }
            catch (Exception)
            {
                return "0";
            }
        }

        public int codigoMunicipio(string municipio, int estado)
        {
            try
            {
                int codigo = (from c in Conexao.CriarEntidade().tab_municipios
                              where c.nome == municipio && c.iduf == estado
                              select c.id).FirstOrDefault();
                return codigo;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int codigoEstado(string uf)
        {
            try
            {
                int codigo = (from u in Conexao.CriarEntidade().estados
                              where u.uf == uf
                              select u.id).FirstOrDefault();
                return codigo;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool validaCliente(int codigo)
        {

            var dadosCliente = (from c in Conexao.CriarEntidade().clientes
                                where c.Codigo == codigo
                                select new { c.cpf, c.cnpj, c.endereco, c.numero, c.bairro, c.cidade, c.estado, c.cep }).FirstOrDefault();

            if (dadosCliente != null)
            {
                bool CNPJ = false;
                bool CPF = false;

                if (dadosCliente.cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim() == ""
                    || dadosCliente.cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim().Length < 14
                    || dadosCliente.cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim().Length > 14)
                {
                    CNPJ = false;
                }
                else
                {
                    CNPJ = true;
                }

                if (dadosCliente.cpf.Replace(".", "").Replace("-", "").Replace("/", "").Trim() == ""
                || dadosCliente.cpf.Replace(".", "").Replace("-", "").Replace("/", "").Trim().Length < 11
                || dadosCliente.cpf.Replace(".", "").Replace("-", "").Replace("/", "").Trim().Length > 11)
                {
                    CPF = false;
                }
                else
                {
                    CPF = true;
                }

                if(CPF == false && CNPJ ==  false)
                {
                    throw new Exception("CPF ou CNPJ Destinatário inválido");
                }
                else if (dadosCliente.cpf.Replace(".", "").Replace("-", "").Replace("/", "").Trim() == "" && (dadosCliente.cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim() == "" || dadosCliente.cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Length < 14))
                {
                    throw new Exception("CNPJ do Cliente inválido");
                }
                else if (dadosCliente.endereco.Trim() != "" && dadosCliente.numero.Trim() == "")
                {
                    throw new Exception("Numero(endereço) do Cliente inválido");
                }
                else if (dadosCliente.endereco.Trim() != "" && dadosCliente.bairro.Trim() == "")
                {
                    throw new Exception("Bairro do Cliente inválido");
                }
                else if (dadosCliente.endereco.Trim() != "" && dadosCliente.cidade.Trim() == "")
                {
                    throw new Exception("Cidade do Cliente inválido");
                }
                else if (codigoEstado(dadosCliente.estado) == 0)
                {
                    throw new Exception("Codigo estado inválido");
                }
                else if (codigoMunicipio(dadosCliente.cidade, codigoEstado(dadosCliente.estado)) == 0)
                {
                    throw new Exception("Codigo do Municipio inválido");
                }
                else if (dadosCliente.endereco.Trim() != "" && dadosCliente.estado.Trim() == "")
                {
                    throw new Exception("UF do Cliente inválido");
                }
                /*else if (dadosCliente.endereco.Trim() != "" && dadosCliente.estado.Trim() != Configuracoes.estado)
                {
                    throw new Exception("UF do destinatario diferente do UF emitente!");
                }*/
                else if(dadosCliente.cep.Trim()== "" || dadosCliente.cep.Replace(".","").Replace("-","").Replace(",","").Replace("/","").Length < 8 ||
                    dadosCliente.cep.Replace(".", "").Replace("-", "").Replace(",", "").Replace("/", "").Length > 8)
                {
                    throw new Exception("CEP do destinatario Invalido!");
                }
                else
                {
                    return true;
                }

            }
            else
            {
                throw new Exception("Dados do Cliente inválido");
            }

        }

        public bool cancelarNFCe(string numero, string serie, string chave, string protocolo, string justificativa)
        {

            if (protocolo == "")
            {
                MessageBox.Show("Documento Sem Protocolo de Autorização", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (chave == "")
            {
                MessageBox.Show("Documento Sem Chave de Autorização", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (serie == "")
            {
                MessageBox.Show("Documento Sem Serie de NFCe", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (numero == "")
            {
                MessageBox.Show("Documento Sem Numero de NFCe", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (FrmObs.observacao.Trim().Length < 15)
            {
                MessageBox.Show("Observação não pode ser menor que 15 caracteres", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

                int retorno = DARUMA_FW.tCFCancelar_NFCe_Daruma(numero, serie, chave, protocolo, justificativa);

                if (retorno < 0)
                {
                    StringBuilder mensagem = new StringBuilder();
                    StringBuilder codigo = new StringBuilder();
                    codigo.Append(retorno);
                    DARUMA_FW.rAvisoErro_NFCe_Daruma(codigo, mensagem);
                    MessageBox.Show(mensagem.ToString());
                    throw new Exception(DARUMA_FW.TrataRetorno(retorno));
                }

                return true;
            }

            return false;
        }

        public bool RelatorioGerencial()
        {
            try
            {
                if (conteudoImpressao.Length > 0)
                {
                    var filial = (from f in Conexao.CriarEntidade().filiais
                                  where f.CodigoFilial == GlbVariaveis.glb_filial
                                  select f).FirstOrDefault();

                    cabecalho.Append(" " + filial.fantasia + "\r\n");
                    cabecalho.Append(filial.empresa + "\r\n");
                    cabecalho.Append(filial.endereco + ", " + filial.numero + ", " + filial.bairro + "\r\n");
                    cabecalho.Append(filial.cidade + " - " + filial.estado + " CEP.:" + filial.cep + "\r\n");
                    cabecalho.Append(filial.telefone1 + " - " + filial.telefone2 + "\r\n");
                    cabecalho.Append("===================================" + "\r\n");
                    cabecalho.Append("CNPJ.: " + filial.cnpj + "   IE.: " + filial.inscricao + "\r\n");
                    cabecalho.Append(DateTime.Now.Date.ToShortDateString() + " : " + DateTime.Now.ToString("hh:mm") + "\r\n");
                    cabecalho.Append("===================================" + "\r\n");

                    rodaPe.Append("===================================" + "\r\n");
                    rodaPe.Append("Usuario.:" + GlbVariaveis.glb_Usuario + "\r\n");
                    rodaPe.Append("SICEpdv Versão " + GlbVariaveis.glb_Versao + "\r\n");
                    rodaPe.Append("Serie.: " + ConfiguracoesECF.NFCserie + " / Terminal.:"+GlbVariaveis.glb_IP+"\r\n");

                    impressao.Append(cabecalho);
                    impressao.Append(conteudoImpressao);
                    impressao.Append(rodaPe);

                    PCPrint printer = new PCPrint();

                    printer.Fonte = new Font("Verdana", 7);
                    printer.PrinterSettings.PrinterName = ConfiguracoesECF.NFCNomeImpressora;

                    printer.Texto = impressao.ToString();
                   
                    printer.Print();
                }
                conteudoImpressao.Clear();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool imprimirCupom(int documento, string tipo = "PDV", bool enviarEmail = true, bool reimpressao = false)
        {
            #region
            /*
            string impressao = "";
            bool fechar;
            List<DANFE.itens> listItens = new List<DANFE.itens>();
            List<DANFE.formasPagamento> listFormaPagamento = new List<DANFE.formasPagamento>();


            if(ConfiguracoesECF.NFCmodImpressao == "P")
                impressao = "PDF";
            else if(ConfiguracoesECF.NFCmodImpressao == "I")
                impressao = "Imagem";
            else if(ConfiguracoesECF.NFCmodImpressao == "R")
                impressao = "Report Viewer";

            if (ConfiguracoesECF.NFCImpressaoDireta == true)
                 fechar = true;
            else
                fechar = false;
            



            var filial = (from f in Conexao.CriarEntidade().filiais
                          where f.CodigoFilial == GlbVariaveis.glb_filial
                          select f).FirstOrDefault();

            var contdocs = (from d in Conexao.CriarEntidade().contdocs
                             where d.documento == documento
                             select d).FirstOrDefault();

            var configuracao = (from m in Conexao.CriarEntidade().configfinanc
                                select m.msgrodapecupom).FirstOrDefault();

            var email = (from c in Conexao.CriarEntidade().clientes
                             where c.cpf == Venda.dadosConsumidor.cpfCnpjConsumidor || c.cnpj == Venda.dadosConsumidor.cpfCnpjConsumidor
                             select c.email).FirstOrDefault();

            if (enviarEmail == true && (email == null || email == "") && contdocs.codigocliente > 0)
            {
                MessageBox.Show("Cliente não tem email cadastrado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                email = "";
            }



            DANFE.FrmDANFE65 objDanfe = new DANFE.FrmDANFE65();
            objDanfe.empFantasia = filial.fantasia;
            objDanfe.empRazaoSocial = filial.empresa;
            objDanfe.empRua = filial.endereco;
            objDanfe.empNumero = filial.numero;
            objDanfe.empBairro = filial.bairro;
            objDanfe.empCidade = filial.cidade;
            objDanfe.empEstado = filial.estado;
            objDanfe.empCep = filial.cep;

            if (tipo == "PDV")
            {
                objDanfe.consCNPJCPF = Venda.dadosConsumidor.cpfCnpjConsumidor;
                objDanfe.consNome = Venda.dadosConsumidor.nomeConsumidor;
                objDanfe.consRua = Venda.dadosConsumidor.endConsumidor;
                objDanfe.consNumero = Venda.dadosConsumidor.endNumero;
                objDanfe.consBairro = Venda.dadosConsumidor.endBairro;
                objDanfe.consCidade = Venda.dadosConsumidor.endCidade;
                objDanfe.consEstado = Venda.dadosConsumidor.endEstado;
                objDanfe.consCEP = Venda.dadosConsumidor.endCEP;
                //objDanfe.desconto = descontoVenda;
            }
            else
            {
                #region Pega dados do cliente

                if (contdocs.codigocliente > 0)
                {
                    var cliente = (from c in Conexao.CriarEntidade().clientes
                                   where c.Codigo == contdocs.codigocliente
                                   select new
                                   {
                                       nome = c.Nome,
                                       cpfCnpj = c.tipo == "FISICA" ? c.cpf : c.cnpj,
                                       endereco = c.endereco,
                                       numero = c.numero,
                                       bairro = c.bairro,
                                       cidade = c.cidade,
                                       estado = c.estado,
                                       cep = c.cep
                                   }).FirstOrDefault();


                    objDanfe.consCNPJCPF = cliente.cpfCnpj;
                    objDanfe.consNome = cliente.nome;
                    objDanfe.consRua = cliente.endereco;
                    objDanfe.consNumero = cliente.numero;
                    objDanfe.consBairro = cliente.bairro;
                    objDanfe.consCidade = cliente.cidade;
                    objDanfe.consEstado = cliente.estado;
                    objDanfe.consCEP = cliente.cep;
                }

                #endregion


                #region itens do cupom
                var itens = (from i in Conexao.CriarEntidade().venda
                             where i.documento == documento
                              && i.cancelado == "N"
                             select new
                             {
                                 codigo = i.codigo,
                                 produto = i.produto,
                                 quantidade = i.quantidade,
                                 unidade = i.unidade,
                                 preco = i.preco,
                                 total = i.total
                             }).ToList();

                if (itens == null)
                {
                    itens = (from i in Conexao.CriarEntidade().vendaarquivo
                                where i.documento == documento
                                && i.cancelado == "N"
                                 select new
                                 {
                                     codigo = i.codigo,
                                     produto = i.produto,
                                     quantidade = i.quantidade,
                                     unidade = i.unidade,
                                     preco = i.preco,
                                     total = i.total
                                 }).ToList();
                }

                foreach (var item in itens)
                {
                    listItens.Add(new DANFE.itens { codigo = item.codigo, descricao = item.produto, quantidade = item.quantidade, unidade = item.unidade, valorUnitario = item.preco.Value, valorTotal = item.total });
                }
                #endregion

                #region Pagamento
                var pagamentos = (from p in Conexao.CriarEntidade().caixa
                                 where p.documento == documento
                                 select new
                                 {
                                     tipopagamento = p.tipopagamento,
                                     valor = p.valor,
                                     desconto = p.vrdesconto
                                 }).ToList();

                if (pagamentos == null)
                {
                    pagamentos = (from p in Conexao.CriarEntidade().caixaarquivo
                                 where p.documento == documento
                                 select new
                                 {
                                     tipopagamento = p.tipopagamento,
                                     valor = p.valor,
                                     desconto = p.vrdesconto
                                 }).ToList();
                }

                //objDanfe.desconto = pagamentos.Sum(x => x.desconto);

                foreach (var pagamento in pagamentos)
                {
                    string descricaoPagamento = " ";

                    switch (pagamento.tipopagamento)
                    {
                        case "DH":
                            descricaoPagamento = ConfiguracoesECF.DH;
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

                    listFormaPagamento.Add(new DANFE.formasPagamento { descricao = descricaoPagamento, valor = pagamento.valor });
                }

                #endregion

            }

            
            objDanfe.hambiente = ConfiguracoesECF.NFCHambiente;
            objDanfe.numero = contdocs.ncupomfiscal.ToString().PadLeft(6,'0');
            objDanfe.serie = contdocs.ecfcontadorcupomfiscal.ToString();
            objDanfe.dataEmissao = DateTime.Now.Date.ToShortDateString();
            objDanfe.horasEmissao = DateTime.Now.Date.ToShortTimeString();
            objDanfe.linkAcesso = "teste marc kvaldo";
            objDanfe.chaveAcesso = contdocs.chaveNFC;
            objDanfe.protocolo = contdocs.protocolo;
            objDanfe.mensagemNFCe = "Vendedor.:"+Venda.vendedor+"\n"+configuracao;
            objDanfe.visualizar = true;
            //objDanfe.fecharImpressao = fechar;
            objDanfe.modImpressao = impressao;
            objDanfe.abrirPDF = false;
            objDanfe.imprimirPDFDireto = true;
            objDanfe.caminhoImpressao = @"C:\iqsistemas\sicepdv\ImpressaoDANFE\";
            objDanfe.caminhoQRCODE = @"C:\iqsistemas\sicepdv\QRcode\";

            if (tipo == "PDV")
            {
                #region itens da venda
                foreach (var item in itens)
                {
                    listItens.Add(new DANFE.itens { codigo = item.codigo, descricao = item.produto, quantidade = item.quantidade, unidade = item.unidade, valorUnitario = item.preco.Value, valorTotal = item.total });
                }

                #endregion


                #region pagamento

                foreach (var pagamento in pagamentos)
                {
                    string descricaoPagamento = " ";

                    switch (pagamento.tipopagamento)
                    {
                        case "DH":
                            descricaoPagamento = ConfiguracoesECF.DH;
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

                    listFormaPagamento.Add(new DANFE.formasPagamento { descricao = descricaoPagamento, valor = pagamento.valor });
                }

                #endregion
            }

            objDanfe.Itens = listItens;
            objDanfe.FormasPagamento = listFormaPagamento;


            if (ConfiguracoesECF.NFCenviarEmail == true && email != "" && enviarEmail == true)
            {
                StringBuilder mensagem = new StringBuilder();

                DANFE.email objEmail = new DANFE.email();
                objEmail.Host = ConfiguracoesECF.NFCHost;
                objEmail.anexo = true;
                objEmail.caminhoArquivo = @"C:\iqsistemas\sicepdv\ImpressaoDANFE\";
                objEmail.conta = ConfiguracoesECF.NFCEmail;
                objEmail.senha = ConfiguracoesECF.NFCSenhaEmail;
                objEmail.Titulo = ConfiguracoesECF.NFCTitulo;
                objEmail.mesagem = mensagem.Append(ConfiguracoesECF.NFCMensagem);
                objEmail.Ssl = ConfiguracoesECF.NFCUsarSSL;
                objEmail.emailDe = ConfiguracoesECF.NFCEmail;
                objEmail.emailPara = email;
                objEmail.porta = int.Parse(ConfiguracoesECF.NFCPortaEmail);
                objDanfe.dadosEmail = objEmail;
            }

            objDanfe.construirDANFE();
            objDanfe.ShowDialog();


            return true;
        }

        public string urlConsultaNFC(string estado, string hambiente)
        {
           
                if (File.Exists("urlConsulta.xml"))
                {
                    try
                    {
                        XDocument xmlTerminal = XDocument.Load("urlConsulta.xml");
                        var url = (from c in xmlTerminal.Descendants("urls").Elements("url")
                                   where c.Element("estado").Value == "SE"
                                   select c.Element("url")).FirstOrDefault();

                        return url.Value.ToString();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("não foi possivel carregar o urlConsulta.xml");
                    }
                }
                return "";
             * */
            #endregion

            if (/*GlbVariaveis.versaoSICENFCe > 66 && */reimpressao == false)
                return true;

            var documentos = (from d in Conexao.CriarEntidade().contdocs
                                 where d.documento == documento
                             select d).FirstOrDefault();


            if (!Permissoes.reimprimirDocumento && reimpressao == true)
            {

                FrmLogon Logon = new FrmLogon();
                Operador.autorizado = false;
                Logon.campo = "outimpcupom";
                Logon.lblDescricao.Text = "REIMPRIMIR DANFe";
                Logon.txtDescricao.Text = GlbVariaveis.glb_Usuario +
                    " REIMPRIMIR DANFe ";
                Logon.ShowDialog();
                if (!Operador.autorizado)
                    return false;
            };




            if (documentos.estornado == "S")
            {
                MessageBox.Show("Não é Possivel Reimprimir um Documento Cancelado!","Atenção",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

            try
            {
                
                if (ConfiguracoesECF.NFCModeloImpressao == "")
                {
                    MessageBox.Show("Não foi possivel verificar o Modelo de Impressão", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }


                if (documentos.chaveNFC.ToString() != "Erro")
                {
                   Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICENFC-e\NFCePrint.exe", " " + documentos.chaveNFC + " " + ConfiguracoesECF.NFCModeloImpressao + " " + ConfiguracoesECF.NFCNomeImpressora + " " + string.Format("{0:N2}", documentos.troco));
                }

                return true;
            }
            catch (Exception erro)
            {
                MessageBox.Show("erro na impressão do NFC-e " + erro.Message);
                return false;
            }
           
        }

        public int LerSequenciaNFCGuardada()
        {
            int sequencia = 0;

            try
            {

                if (!File.Exists(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml"))
                    return 0;

                XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml");
                var config = from c in xmlTerminal.Descendants("NFC")
                             select new
                             {
                                 Sequencia = c.Element("SequenciaNFC").Value
                             };

                foreach (var item in config)
                {
                    sequencia = int.Parse(item.Sequencia.ToString());
                }

                

            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível carregar o arquivo NFC.XML. Refaça-o :" + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }

            return sequencia;
        }

        public void CriarSequnciaNFC(string sequenciaNFC)
        {
            try
            {
                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "true"),
                    new XElement("NFC",
                            new XElement("SequenciaNFC", sequenciaNFC)
                            ));
                doc.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml");
            }
            catch (Exception erro)
            {
                MessageBox.Show("Não foi possível criar o arquivo Terminal.xml " + erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GuardarSequenciaNFC(string sequenciaNFC)
        {
            XDocument xmlTerminal = XDocument.Load(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml");
            var config = from c in xmlTerminal.Descendants("NFC")
                         //where c.Element("numeroECF").Value =="1"
                         select c;

            foreach (var item in config)
            {
                item.Element("SequenciaNFC").Value = sequenciaNFC;
            }
            xmlTerminal.Save(@ConfigurationManager.AppSettings["dirArquivoAuxiliar"] + @"\SequenciaNFC.xml");
        }

        public bool GerarReq(string numeroNFe, string serieNF, string documento, string acao, string ip ,string justificativa="")
        {
            string SQL = "";

            try
            {
                if (serieNF.Length < 4)
                    serieNF = serieNF.PadLeft(3, '0');
                else
                    serieNF = serieNF.Substring(serieNF.Length - 3, 3);

                string req = numeroNFe.PadLeft(11, '0') + serieNF + documento.PadLeft(11, '0') + acao + ip.PadLeft(15, '0') + justificativa.PadRight(60, ' ');
                if (ConfiguracoesECF.NFCtransacaobanco == false)
                {
                    if (File.Exists(@"C:\iqsistemas\SICENFC-e\Req\nfce.txt"))
                    {
                        File.Delete(@"C:\iqsistemas\SICENFC-e\Req\nfce.txt");
                    }

                   

                    File.WriteAllText(@"C:\iqsistemas\SICENFC-e\Req\nfce.txt", req);
                    if (justificativa.Trim() != "")
                        FuncoesNFC.verificarGerenciadorNFCe();
                    return true;
                }
                else
                {
                    siceEntities entidade = Conexao.CriarEntidade();

                    //var transacao = entidade.ExecuteStoreQuery<int>("select count(1) as quantidade from transacoesnfce where ip = '" + GlbVariaveis.glb_IP + "' and codigofilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();
                    int quantidadeTransacao = (from t in Conexao.CriarEntidade().transacoesnfce
                                                where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                                select t).ToList().Count();

                    if (quantidadeTransacao > 0)
                    {

                        var transacao = (from t in entidade.transacoesnfce
                                         where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                         select t).ToList();
                        foreach (var item in transacao)
                        {
                            entidade.transacoesnfce.DeleteObject(item);
                            entidade.SaveChanges();
                        }
                    }

                    /*SQL = "insert into transacoesnfce(codigofilial,ip,req,resp,erro) values ('" + GlbVariaveis.glb_filial + "','" + GlbVariaveis.glb_IP + "','" + req + "','','')";

                    using (var context = Conexao.CriarEntidade())
                    {
                        context.ExecuteStoreCommand(SQL);
                    }*/
                    
                    transacoesnfce novo = new transacoesnfce();
                    novo.codigofilial = GlbVariaveis.glb_filial;
                    novo.ip = GlbVariaveis.glb_IP;
                    novo.req = req;
                    novo.resp = "";
                    novo.erro = "";
                    

                    entidade.transacoesnfce.AddObject(novo);
                    entidade.SaveChanges();

                    Thread.Sleep(2000);
                    
                     return true;
                }
            }
            catch(Exception erro)
            {
                MessageBox.Show(erro.Message);
                return false;
            }
        }

        public dadosNFCe LerResp(string acao = "Venda", bool mostrarMsg = true)
        {
            string numeroItem = "";
            int i = 0;
            while (true)
            {
                if (i == 500)
                {
                    FuncoesNFC.verificarGerenciadorNFCe();
                    i = 0;
                }

                i++;

                try
                {
                    string tipo = "";
                    string chave = "";
                    string protocolo = "";
                    string data = "";
                    string hora = "";
                    string line = "";

                    if (ConfiguracoesECF.NFCtransacaobanco == false)
                    {
                        #region arquivo 

                        if (acao == "cancelamento")
                            FuncoesNFC.verificarGerenciadorNFCe();

                        if (File.Exists(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt"))
                        {
                            
                            StreamReader file = new StreamReader(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");
                            while ((line = file.ReadLine()) != null)
                            {
                                tipo = line.Substring(0, 1);
                                chave = line.Substring(1, 44);
                                protocolo = line.Substring(45, 15);
                                data = line.Substring(60, 10);
                                hora = line.Substring(71, 8);
                            }

                            dadosNFCe nfc = new dadosNFCe();
                            nfc.chaveNFe = chave;
                            nfc.protocolo = protocolo;
                            nfc.acao = tipo;
                            nfc.horaAutorizacao = hora;
                            nfc.dataAutorizacao = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(5, 2)), int.Parse(data.Substring(8, 2)));
                            nfc.numeroNF = chave.Substring(25, 9);
                            nfc.serieNF = chave.Substring(22, 3);

                            file.Close();

                            return nfc;
                        }


                        if (File.Exists(@"C:\iqsistemas\SICENFC-e\Resp\erro.txt"))
                        {
                            string linha;
                            string DescricaoErroCompleto = "";
                            StreamReader file = new StreamReader(@"C:\iqsistemas\SICENFC-e\Resp\erro.txt");
                            while ((linha = file.ReadLine()) != null)
                            {
                                DescricaoErroCompleto = DescricaoErroCompleto + linha;
                            }

                            frmOperadorTEF msg = new frmOperadorTEF(DescricaoErroCompleto, false);
                            msg.ShowDialog();
                            msg.Dispose();
                            dadosNFCe nfc = new dadosNFCe();
                            nfc.chaveNFe = "Erro";
                            nfc.protocolo = "Erro";
                            nfc.acao = "Erro";
                            nfc.horaAutorizacao = "";
                            nfc.dataAutorizacao = DateTime.Now.Date;
                            nfc.numeroNF = "Erro";
                            nfc.serieNF = "Erro";
                            file.Close();
                            File.Delete(@"C:\iqsistemas\SICENFC-e\Resp\erro.txt");
                            return nfc;
                        }

                        #endregion
                    }
                    else
                    {
                        #region base dados 

                        siceEntities entidade = Conexao.CriarEntidade();

                        var resp = entidade.ExecuteStoreQuery<string>("select resp from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "' and resp <> '' and erro = ''").FirstOrDefault();

                        //MessageBox.Show(resp);

                        if (resp != null)
                        {
                            tipo = resp.ToString().Trim().Substring(0, 1);
                            chave = resp.ToString().Trim().Substring(1, 44);
                            protocolo = resp.ToString().Trim().Substring(45, 15);
                            data = resp.ToString().Trim().Substring(60, 10);
                            hora = resp.ToString().Trim().Substring(71, 8);

                            dadosNFCe nfc = new dadosNFCe();
                            nfc.chaveNFe = chave;
                            nfc.protocolo = protocolo;
                            nfc.acao = tipo;
                            nfc.horaAutorizacao = hora;
                            nfc.dataAutorizacao = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(5, 2)), int.Parse(data.Substring(8, 2)));
                            nfc.numeroNF = chave.Substring(25, 9);
                            nfc.serieNF = chave.Substring(22, 3);
                            nfc.codigoProduto = "";
                            return nfc;
                        }

                        var erro = entidade.ExecuteStoreQuery<string>("select erro from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "' and erro <> ''").FirstOrDefault();
                        string MsgemErro = "";

                        if (erro != null)
                        {
                            #region
                            if(erro.ToString().Contains("nItem") == true)
                            {
                                try
                                {
                                    MsgemErro = erro;
                                    erro = erro.Replace("\n", "").Replace("\r", "").ToLower();
                                    //erro = "778-Regeição: informado o NCM inexistente [nitem:43]";
                                    //MessageBox.Show(erro.IndexOf("[nitem:").ToString());
                                    //MessageBox.Show(erro.IndexOf("nitem=").ToString());
                                    int index = erro.IndexOf("nitem=");
                                    int limit;

                                    if(index < 0)
                                    {
                                        index = erro.IndexOf("[nitem:");
                                    }

                                    if((erro.Length - index) > 10)
                                    {
                                        limit = 10;
                                    }
                                    else
                                    {
                                        limit = (erro.Length - index);
                                    }

                                    //erro = erro.Substring(erro.Length - 10, 10).ToLower();
                                    erro = erro.Substring(index, limit).ToLower();
                                    //=\"1\">
                                    erro = erro.Replace("nitem", "").Replace("[","").Replace("]","").Replace("item","").Replace("nite","").Replace(":","").Replace(" ","").Replace("=", "").Replace(@"\", "").Replace("\"", "");
                                    numeroItem = erro;
                                }
                                catch
                                {
                                    numeroItem = "";
                                }
                            }
                            else
                            {
                                MsgemErro = erro;
                            }

                            if (mostrarMsg == true)
                            {
                                
                                frmOperadorTEF msg = new frmOperadorTEF(MsgemErro.ToString().Trim(), false);
                                msg.ShowDialog();
                                msg.Dispose();
                            }

                            dadosNFCe nfc = new dadosNFCe();
                            nfc.chaveNFe = "Erro";
                            nfc.protocolo = "Erro";
                            nfc.acao = "Erro";
                            nfc.horaAutorizacao = "";
                            nfc.dataAutorizacao = DateTime.Now.Date;
                            nfc.numeroNF = "Erro";
                            nfc.serieNF = "Erro";
                            nfc.codigoProduto = numeroItem;
                            entidade.ExecuteStoreCommand("delete from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "'");
                            return nfc;
                            #endregion
                        }

                        var quantNFCe = entidade.ExecuteStoreQuery<int>("select count(1) as quantidade from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "'").FirstOrDefault();

                        if(quantNFCe == 0)
                        {
                            dadosNFCe nfc = new dadosNFCe();
                            nfc.chaveNFe = "Erro";
                            nfc.protocolo = "Erro";
                            nfc.acao = "Erro";
                            nfc.horaAutorizacao = "";
                            nfc.dataAutorizacao = DateTime.Now.Date;
                            nfc.numeroNF = "Erro";
                            nfc.serieNF = "Erro";
                            nfc.codigoProduto = "";
                            return nfc;
                        }
                        #endregion
                    }
                }
                catch (Exception erro)
                {
                    throw new Exception(erro.ToString());
                }

                
            }
        }

        public dadosNFCe verificaResp()
        {
            string tipo = "";
            string chave = "";
            string protocolo = "";
            string data = "";
            string hora = "";
            string line = "";

            if (ConfiguracoesECF.NFCtransacaobanco == false)
            {
                #region
                if (File.Exists(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt"))
                {
                   
                    StreamReader file = new StreamReader(@"C:\iqsistemas\SICENFC-e\Resp\resp.txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        tipo = line.Substring(0, 1);
                        chave = line.Substring(1, 44);
                        protocolo = line.Substring(45, 15);
                        data = line.Substring(60, 10);
                        hora = line.Substring(71, 8);
                    }

                    dadosNFCe nfc = new dadosNFCe();
                    nfc.chaveNFe = chave;
                    nfc.protocolo = protocolo;
                    nfc.acao = tipo;
                    nfc.horaAutorizacao = hora;
                    nfc.dataAutorizacao = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(5, 2)), int.Parse(data.Substring(8, 2)));
                    nfc.numeroNF = chave.Substring(25, 9);
                    nfc.serieNF = chave.Substring(22, 3);

                    file.Close();

                    return nfc;
                }
                else
                {
                    dadosNFCe nfc = new dadosNFCe();
                    nfc.numeroNF = "0";
                    return nfc;
                }

                #endregion
            }
            else
            {
                #region base dados

                var resp = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select resp from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "' and resp <> '' and erro = ''").FirstOrDefault();

                //MessageBox.Show("1");
                //MessageBox.Show(resp.ToString()+"teste");
                //MessageBox.Show(GlbVariaveis.glb_IP);

                if (resp != null)
                {
                    try
                    {
                        tipo = resp.ToString().Trim().Substring(0, 1);
                        chave = resp.ToString().Trim().Substring(1, 44);
                        protocolo = resp.ToString().Trim().Substring(45, 15);
                        data = resp.ToString().Trim().Substring(60, 10);
                        hora = resp.ToString().Trim().Substring(71, 8);

                        dadosNFCe nfc = new dadosNFCe();
                        nfc.chaveNFe = chave;
                        nfc.protocolo = protocolo;
                        nfc.acao = tipo;
                        nfc.horaAutorizacao = hora;
                        nfc.dataAutorizacao = new DateTime(int.Parse(data.Substring(0, 4)), int.Parse(data.Substring(5, 2)), int.Parse(data.Substring(8, 2)));
                        nfc.numeroNF = chave.Substring(25, 9);
                        nfc.serieNF = chave.Substring(22, 3);
                        return nfc;
                    }
                    catch (Exception erro)
                    {
                       throw new Exception(erro.ToString());
                    }
                }
                else
                {
                    dadosNFCe nfc = new dadosNFCe();
                    nfc.numeroNF = "0";
                    return nfc;
                }
                #endregion
            }
        }

        public void excluirDadosTransacoesNfce(String acao)
        {
            List<transacoesnfce> transacao = null;

            // Excluir dados da transacoesnfce
            siceEntities entidade = Conexao.CriarEntidade();

            int quantidadeTransacao = (from t in Conexao.CriarEntidade().transacoesnfce
                                        where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                        select t).ToList().Count();

            if (quantidadeTransacao > 0)
            {
                if (acao == "INUTILIZACAO DE FAIXA")
                {
                    transacao = (from t in entidade.transacoesnfce
                                 where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial && t.req.Contains(acao)
                                 select t).ToList();
                }
                else if (acao == "Inutilizacao de numero homologado")
                {
                    transacao = (from t in entidade.transacoesnfce
                                 where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial && t.resp.Contains(acao)
                                 select t).ToList();
                }
                else if (acao == "Rejeicao")
                {
                    transacao = (from t in entidade.transacoesnfce
                                 where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial && t.erro.Contains(acao)
                                 select t).ToList();
                }

                foreach (var item in transacao)
                {
                    entidade.transacoesnfce.DeleteObject(item);
                    entidade.SaveChanges();
                }
            }
        }

        public bool inutilizarNFCeReq(string codigoFilial, string numeroInicioNFe, string numeroFimNfe, string serieNF, string documento, string acao, string ipTerminal, string justificativa)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            String filial = codigoFilial;
            String ip = ipTerminal;
            String numeroNFe = numeroInicioNFe;
            String numeroFinal = numeroFimNfe;

            if (serieNF.Length < 4)
                serieNF = serieNF.PadLeft(3, '0');
            else
                serieNF = serieNF.Substring(serieNF.Length - 3, 3);

            string req = numeroNFe.PadLeft(11, '0') + serieNF + documento.PadLeft(11, '0') + acao + ip.PadLeft(15, '0') + justificativa.PadRight(60, ' ');


            String SQL = "INSERT INTO `transacoesnfce`(`codigofilial`,`resp`,`req`,`ip`,`erro`,`numerofinal`) VALUES('" + filial + "', '', " +
                         "'"+ req + "', '" + ip + "', '', '" + numeroFinal + "');";


            int quantidadeTransacao = (from t in Conexao.CriarEntidade().transacoesnfce
                                       where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                       select t).ToList().Count();

            if (quantidadeTransacao > 0)
            {

                var transacao = (from t in entidade.transacoesnfce
                                 where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                 select t).ToList();
                foreach (var item in transacao)
                {
                    entidade.transacoesnfce.DeleteObject(item);
                    entidade.SaveChanges();
                }
            }

            // Gravar a inutilização na transacoesnfce
            using (var context = Conexao.CriarEntidade())
            {
                try
                {
                    context.ExecuteStoreCommand(SQL);
                }
                catch (Exception erro)
                {
                    throw new Exception(erro.ToString());
                }
                
            }

            return true;

        }

        public string inutilizarNFCeResp()
        {
            siceEntities entidade = Conexao.CriarEntidade();

            while (true)
            {
                var resp = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select resp from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "' and resp <> '' and erro = '';").FirstOrDefault();

                if (resp != null)
                {
                    int quantidadeTransacao = (from t in Conexao.CriarEntidade().transacoesnfce
                                               where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                               select t).ToList().Count();

                    if (quantidadeTransacao > 0)
                    {

                        var transacao = (from t in entidade.transacoesnfce
                                         where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                         select t).ToList();
                        foreach (var item in transacao)
                        {
                            entidade.transacoesnfce.DeleteObject(item);
                            entidade.SaveChanges();
                        }
                    }

                    return resp;
                }


                var erro = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select erro from transacoesnfce where codigofilial = '" + GlbVariaveis.glb_filial + "' and ip = '" + GlbVariaveis.glb_IP + "' and erro <> ''").FirstOrDefault();

                if (erro != null)
                {
                    int quantidadeTransacao = (from t in Conexao.CriarEntidade().transacoesnfce
                                               where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                               select t).ToList().Count();

                    if (quantidadeTransacao > 0)
                    {

                        var transacao = (from t in entidade.transacoesnfce
                                         where t.ip == GlbVariaveis.glb_IP && t.codigofilial == GlbVariaveis.glb_filial
                                         select t).ToList();
                        foreach (var item in transacao)
                        {
                            entidade.transacoesnfce.DeleteObject(item);
                            entidade.SaveChanges();
                        }
                    }
                    return erro;
                }
            }
        }

        static public void verificarGerenciadorNFCe(string acao = "Abrir", bool impressao = true)
        {
            try
            {
                //if (ConfiguracoesECF.NFCtransacaobanco == false)
                //{

                    if (acao == "Abrir")
                    {

                        if (ConfiguracoesECF.NFC == true && ConfiguracoesECF.idNFC == 2)
                        {

                        
                            bool podeIniciar = true;
                            System.Threading.Mutex primeiraInstanciaSICENFCe = new System.Threading.Mutex(true, "SICENFCe", out podeIniciar);
                            var processoNFCe = from n in Process.GetProcesses()
                                               where (n.ProcessName.Contains("SICENFCe") || n.ProcessName.Contains("sicenfce")
                                               || n.ProcessName.Contains("sicenfce.exe") || n.ProcessName.Contains("sicenf")
                                               || n.ProcessName.Contains("SICENF"))
                                               //&& !n.ProcessName.Contains("SICEpdv.vs")
                                               select n;
                        

                            if (processoNFCe.Count() > 0)
                                podeIniciar = false;

                            if (podeIniciar==true)
                            {

                                string impressora = ConfiguracoesECF.NFCNomeImpressora;

                                if (impressao == false)
                                    impressora = "NOPRINT";

                                //MessageBox.Show(GlbVariaveis.versaoSICENFCe.ToString());
                                //MessageBox.Show(ConfiguracoesECF.NFCNomeImpressora.ToString());

                                //if (GlbVariaveis.versaoSICENFCe <= 66)
                                //{
                                    //Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICENFC-e\SICENFCe.exe");
                                //}
                                //else
                                //{
                                    Process myProcess = System.Diagnostics.Process.Start(@"C:\iqsistemas\SICENFC-e\SICENFCe.exe", " "+ impressora);
                                //}
                            }

                            
                            
                        }
                    }
                    else if(acao == "Fechar")
                    {
                        Process[] processes = Process.GetProcessesByName("SICENFCe");

                        foreach (Process process in processes)
                        {
                            process.Kill();
                        }
                    }

                   
                //}
            }
            catch //(Exception erro)
            {
                //throw new Exception("Não foi possivel verificar o SICENFC-e nesse Terminal");
            }
        }

        static public List<CaixaFiscal> valoresCaixaNFCe(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            if (autorizado == false)
            {
                string SQL = "SELECT i.CbdCST AS tributacao, i.CbdpICMS AS icms, TRUNCATE(SUM(v.CbdvProd - v.CbdvDesc), 2) AS valorProdutos,TRUNCATE(SUM(i.CbdvBC), 2) as baseICMS, TRUNCATE(SUM(i.CbdvICMS_icms), 2) AS imposto FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                                                                                "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                                                                                "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND i.CbdnItem = v.CbdnItem " +
                                                                                "AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'" +
                                                                                "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                                                                                "AND c.cbdcancelado = 'N' GROUP BY i.CbdCST, i.CbdpICMS";
                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<CaixaFiscal>(SQL).ToList();
            }
            else
            {
                string SQL = "SELECT i.CbdCST AS tributacao, i.CbdpICMS AS icms, TRUNCATE(SUM(v.CbdvProd - v.CbdvDesc), 2) AS valorProdutos,TRUNCATE(SUM(i.CbdvBC), 2) as baseICMS, TRUNCATE(SUM(i.CbdvICMS_icms), 2) AS imposto FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                                                                               "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                                                                               "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                                                                               "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                                                                               "AND i.CbdnItem = v.CbdnItem " +
                                                                               "AND c.CbdNumProtocolo IS NOT NULL "+
                                                                               "AND c.CbdNumProtocolo <> 0 " +
                                                                               "AND c.CbdCodigoFilial = '"+GlbVariaveis.glb_filial+"'" +
                                                                               "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                                                                               "AND c.cbdcancelado = 'N' GROUP BY i.CbdCST, i.CbdpICMS";
                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<CaixaFiscal>(SQL).ToList();
            }
            
        }

        static public List<DateTime> datasNFCe(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            if (autorizado == false)
            {
                string SQL = "SELECT c.cbddVenda AS DATA FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                            "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                            "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                            "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                            "AND i.CbdnItem = v.CbdnItem " +
                            "AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'" +
                            "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                            "AND c.cbdcancelado = 'N' GROUP BY c.cbddVenda";

                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(SQL).ToList();
            }
            else
            {
                string SQL = "SELECT c.cbddVenda AS DATA FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                            "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                            "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                            "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                            "AND i.CbdnItem = v.CbdnItem " +
                            "AND c.CbdNumProtocolo IS NOT NULL " +
                            "AND c.CbdNumProtocolo <> 0 " +
                            "AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'" +
                            "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                            "AND c.cbdcancelado = 'N' GROUP BY c.cbddVenda";

                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<DateTime>(SQL).ToList();
            }

        }

        static public List<CaixaFiscal> valoresCaixaCanceladoNFCe(DateTime dataIncio, DateTime dataFinal, bool autorizado = false)
        {
            if (autorizado == true)
            {
                string SQL = "SELECT i.CbdCST AS tributacao, i.CbdpICMS AS icms, TRUNCATE(SUM(v.CbdvProd - v.CbdvDesc), 2) AS valorProdutos,TRUNCATE(SUM(i.CbdvBC), 2) as baseICMS, TRUNCATE(SUM(i.CbdvICMS_icms), 2) AS imposto FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                                                                                "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                                                                                "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND i.CbdnItem = v.CbdnItem " +
                                                                                "AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'" +
                                                                                "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                                                                                "AND c.cbdcancelado = 'S' GROUP BY i.CbdCST, i.CbdpICMS";
                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<CaixaFiscal>(SQL).ToList();
            }
            else
            {
                string SQL = "SELECT i.CbdCST AS tributacao, i.CbdpICMS AS icms, TRUNCATE(SUM(v.CbdvProd - v.CbdvDesc), 2) AS valorProdutos,TRUNCATE(SUM(i.CbdvBC), 2) as baseICMS, TRUNCATE(SUM(i.CbdvICMS_icms), 2) AS imposto FROM cbd001 AS c, cbd001deticmsnormalst AS i, cbd001det AS v " +
                                                                                "WHERE c.cbddVenda BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' AND '" + dataFinal.ToString("yyyy-MM-dd") + "'  " +
                                                                                "AND i.CbdNtfNumero = c.CbdNtfNumero AND i.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND v.CbdNtfNumero = c.CbdNtfNumero AND v.CbdNtfSerie = c.CbdNtfSerie " +
                                                                                "AND i.CbdnItem = v.CbdnItem " +
                                                                                "AND c.CbdNumProtocolo IS NOT NULL " +
                                                                                "AND c.CbdNumProtocolo <> 0 " +
                                                                                "AND c.CbdCodigoFilial = '" + GlbVariaveis.glb_filial + "'" +
                                                                                "AND c.CbdNtfSerie = '" + int.Parse(ConfiguracoesECF.NFCserie) + "' " +
                                                                                "AND c.cbdcancelado = 'S' GROUP BY i.CbdCST, i.CbdpICMS";
                LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
                return Conexao.CriarEntidade().ExecuteStoreQuery<CaixaFiscal>(SQL).ToList();

            }
        }

        static public decimal encargosCaixaNFCe(DateTime dataIncio, DateTime dataFinal)
        {
            string SQL = "SELECT IFNULL(SUM(encargos),0) FROM contdocs AS v WHERE v.DATA = (select ifnull(max(data),current_date) from caixa where enderecoip = '" + GlbVariaveis.glb_IP + "' and dpfinanceiro = 'Venda') AND v.codigoFilial = '" + GlbVariaveis.glb_filial + "' AND estornado = 'N' " +
                                                                      "AND v.modelodocfiscal = '65' AND v.id = '" + GlbVariaveis.glb_IP + "' AND chaveNFC <> 'Error' AND dpfinanceiro = 'Venda' and data BETWEEN '" + dataIncio.ToString("yyyy-MM-dd") + "' and '" + dataFinal.ToString("yyyy-MM-dd") + "' ";
            LogSICEpdv.Registrarlog(SQL, "0", "FuncoesNFC.cs");
            return Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(SQL).FirstOrDefault();
        }

        static public string lerChaveNFCe(string Chave, string tipo = "N")
        {
            if(Chave.Length <44)
            {
                return "0";
            }
            else
            {
                if(tipo == "S")
                    return Chave.Substring(22, 3);
                else
                    return Chave.Substring(25, 9);
            }
        }

        static public bool ajustarNFCeChave(int documento)
       {
            var doc = (from n in Conexao.CriarEntidade().contdocs
                       where n.documento == documento
                       select n).FirstOrDefault();

            string notaFiscal = lerChaveNFCe(doc.chaveNFC,"N");
            string SerieNF = lerChaveNFCe(doc.chaveNFC, "S");

            string SQL = "update contdocs set ecfcontadorcupomfiscal = LPAD('" + SerieNF + "',3,'0'), ncupomfiscal = LPAD('" + notaFiscal + "',9,'0') where documento = '" + documento + "'";

            Conexao.CriarEntidade().ExecuteStoreCommand(SQL);

            return true;

       }
    }

    class conteudo : PrintDocument
    {
        public StringBuilder linha { get; set; }
        public conteudo(StringBuilder _linha)
        {
            this.linha = _linha;
        }

    }

    
}
