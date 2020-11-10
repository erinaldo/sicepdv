using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Windows.Forms;
using System.Data.EntityClient;
using System.Data;

namespace SICEpdv
{
    // Emissão de Arquivo Eletrônico ICMS 57/95

    class Sintegra : dadosFiscaisEmpresa
    {
        public int codigoLayout { get; set; }
        public int codigoNaturezaOperacao { get; set; }
        public int codigoFinalidade { get; set; }
        public string filial { get; set; }

        public int numeroInventario { get; set; }
        public int anoInventario { get; set; }
        public string nomeArquivo = "sintegra.txt";
        private int contadorReg10 = 1;
        private int contadorReg11 = 1;
        private int contadorReg50 = 0;
        private int contadorReg60 = 0;
        private int contadorReg61 = 0;
        private int contadorReg54 = 0;
        private int contadorReg75 = 0;
        private int contadorReg74 = 0;



        private List<string> codigos = new List<string>();
        List<string> codigoParticipante = new List<string>();
        public StringBuilder listaErros = new StringBuilder();
        StringBuilder reg54Itens = new StringBuilder();

        public Sintegra()
        {
            nomeArquivo = @"C:\iqsistemas\sice.net\exportacao\SEF\Sintegra.txt";
            codigoLayout = 3;
            codigoNaturezaOperacao = 3;
            codigoFinalidade = 1;
            filial = "00001";
            ObterDados();
            Funcoes.ProcedureAjuste("AjustarCamposNulos");            
        }


        StringBuilder conteudo = new StringBuilder();
        siceEntities entidade = Conexao.CriarEntidade();

        public void ObterDados()
        {
            var dados = (from n in Conexao.CriarEntidade().filiais
                         where n.CodigoFilial == GlbVariaveis.glb_filial
                         select n).First();
            nomeEmpresa = dados.empresa;
            cnpj = dados.cnpj;
            cpf = "";
            uf = dados.estado;
            ie = dados.inscricao;
            cod_mun = Funcoes.RetornaCodigoMunIBGE("M", dados.cidade, dados.estado);
            im = dados.inscricaomunicipal;
            suframa = "";
            fantasia = dados.descricao;
            cep = dados.cep;
            cidade = dados.cidade;
            estado = dados.estado;
            end = dados.endereco;
            num = dados.numero;
            comp = dados.complemento == null ? "" : dados.complemento;
            bairro = dados.bairro;
            fone = dados.telefone1;
            fax = dados.telefone2;
            email = dados.email;
            contato = dados.contador;
            responsavel = Funcoes.RetirarAcentos(dados.responsavel);
            cpfresponsavel = dados.cpfresponsavel;
            // Contador
            nomeContador = Funcoes.RetirarAcentos(dados.contador);
            cpfContador = dados.cpfcontador;
            CRC = dados.crccontador;
            cnpjContador = dados.cnpjcontador;
            cepContador = dados.cepcontador;
            endContador = dados.enderecocontador;
            EndNumeroContador = dados.numerocontador;
            complementoContador = dados.complementocontador;
            bairroContador = dados.bairrocontador;
            foneContador = dados.telefonecontador;
            faxContador = dados.faxcontador;
            emailContador = dados.emailcontador;
            ind_ativ = dados.indicadoratividade.Substring(0, 1);
        }

        public bool GerarPAF()
        {
            try
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial.Date, dataFinal.Date, GlbVariaveis.glb_filial);
                Registro10();
                Registro11();
                //Registro50Entrada();
                Registro50Saidas();
                conteudo.Append(reg54Itens);
                Registro60();
                Registro61();
                Registro75();
                Registro90();
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando Sintegra: " + erro.Message);
            }

            return true;

        }

        public bool GerarEntradas()
        {
            try
            {
                Registro10();
                Registro11();
                Registro50Entrada();
                conteudo.Append(reg54Itens);
                Registro75();
                Registro90();
                return true;
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando entradas: " + erro.Message);
            }
        }

        public bool GerarSaidas()
        {
            try
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);
                Registro10();
                Registro11();
                Registro50Saidas();
                conteudo.Append(reg54Itens);
                Registro75();
                Registro90();
                return true;
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando saídas: " + erro.Message);
            }
        }

        public bool GerarMapaResumo()
        {
            try
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);
                Registro10();
                Registro11();
                Registro60();
                Registro61();
                Registro75();
                Registro90();
                return true;
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando mapa resumo: " + erro.Message);
            }
        }

        public bool GerarInventario()
        {
            try
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);
                Registro10();
                Registro11();
                Registro74();
                Registro75();
                Registro90();
                return true;
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando inventário: " + erro.Message);
            }
        }

        public bool GeraTodos()
        {
            try
            {
                Registro10();
                Registro11();
                Registro50Entrada();
                Registro50Saidas();
                conteudo.Append(reg54Itens);
                Registro60();
                Registro61();
                Registro75();
                Registro90();
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando Sintegra: " + erro.Message);
            }

            return true;

        }

        public void Registro10()
        {
            conteudo.AppendLine("10" +
                cnpj.PadRight(14, ' ').Substring(0, 14) +
                ie.PadRight(14, ' ').Substring(0, 14) +
                nomeEmpresa.PadRight(35, ' ').Substring(0, 35) +
                cidade.PadRight(30, ' ').Substring(0, 30) +
                estado +
                fax.PadRight(10, '0').Substring(0, 10) +
                string.Format("{0:yyyyMMdd}", dataInicial.Date) +
                string.Format("{0:yyyyMMdd}", dataFinal.Date) +
                codigoLayout.ToString() + // Estrutura confomre Convenio ICMS 57/95 cod_ver+
                codigoNaturezaOperacao.ToString() +
                codigoFinalidade.ToString());
        }

        public void Registro11()
        {
            conteudo.AppendLine("11" +
                end.PadRight(34, ' ').Substring(0, 34) +
                num.PadLeft(5, '0').Substring(0, 5) +
                comp.PadRight(22, ' ').Substring(0, 22) +
                bairro.PadRight(15, ' ').Substring(0, 15) +
                cep.PadRight(8, ' ').Substring(0, 8) +
                contato.PadRight(28, ' ').Substring(0, 28) +
                fone.PadLeft(12, '0').Substring(0, 12));
        }

        public void Registro50Entrada()
        {
            var dados = (from n in entidade.registro50entradas_agr
                         where n.dataentrada >= dataInicial.Date && n.dataentrada <= dataFinal
                         && n.lancada == "X"
                         && n.codigofilial == filial
                         && n.nf != ""
                         && (n.modelonf.Contains("55") || (n.modelonf.Contains("01")
                         || n.modelonf.Contains("06")
                         || n.modelonf.Contains("21")
                         || n.modelonf.Contains("22")))
                         orderby n.numero
                         select new { n.numero, n.nf, n.dataentrada, n.bcicms, n.totalNF, n.toticms, n.cfopentrada, n.icmsentrada });
            var registro50 = dados.ToList().Distinct();

            List<int> numerosProcessados = new List<int>();
            foreach (var item in registro50)
            {
                try
                {

                    var dadoNF = (from n in entidade.moventradas
                                  where n.numero == item.numero
                                  && n.NF == item.nf
                                  && n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal.Date
                                  select n).FirstOrDefault();

                    if (!codigoParticipante.Contains("F" + dadoNF.codigofornecedor.ToString()))
                        codigoParticipante.Add("F" + dadoNF.codigofornecedor.ToString());

                    var infoNF = dadoNF;

                    var dadoEmitente = (from n in entidade.fornecedores
                                        where n.Codigo == infoNF.codigofornecedor
                                        select n).FirstOrDefault();
                    var infoEmitente = dadoEmitente;

                    // Outras - Valor que não confira débito ou crédito do ICMS

                    decimal valorOutras = 0;
                    if (item.bcicms == 0)
                        valorOutras = item.totalNF.Value;


                    // Somando os totais da Nota Fiscal
                    decimal? totalNF = (from n in registro50
                                        where n.numero == item.numero
                                        select n.totalNF).Sum().Value;
                    decimal? totalBCIcms = (from n in registro50
                                            where n.numero == item.numero
                                            select n.bcicms).Sum();
                    decimal? totalICMS = (from n in registro50
                                          where n.numero == item.numero
                                          select n.toticms).Sum();

                    decimal? totalOutrasAliquotas = (from n in registro50
                                                     where n.numero == item.numero
                                                     && n.bcicms == 0
                                                     select (decimal?)n.totalNF).Sum();

                    if (!totalOutrasAliquotas.HasValue)
                        totalOutrasAliquotas = 0;

                    string ieRegistro = infoEmitente.INSCRICAO.Trim();
                    if (string.IsNullOrEmpty(ieRegistro))
                        ieRegistro = "ISENTO";

                    string situacaoNF = "N";
                    if (infoNF.situacaoNF == "02" || infoNF.NF == "S" || infoNF.situacaoNF == "03" || infoNF.situacaoNF == "04" || infoNF.situacaoNF == "05")
                        situacaoNF = "S";

                    conteudo.AppendLine("50" +
                        (infoEmitente.CGC + infoEmitente.CPF).PadRight(14, ' ').Substring(0, 14) +
                        ieRegistro.Replace(".", "").Replace("-", "").PadRight(14, ' ').Substring(0, 14) +
                        string.Format("{0:yyyyMMdd}", item.dataentrada) +
                        infoEmitente.ESTADO +
                        infoNF.modeloNF.PadRight(2, ' ').Substring(0, 2) +
                        infoNF.serie.PadRight(3, ' ').Substring(0, 3) +
                        infoNF.NF.PadLeft(6, '0').Substring(0, 6) +
                        item.cfopentrada.Replace(".", "").PadLeft(4, '0').Substring(0, 4) +
                        infoNF.Emitente +
                        Funcoes.FormatarZerosEsquerda(item.totalNF.Value, 13, true) +
                        Funcoes.FormatarZerosEsquerda(item.bcicms.Value, 13, true) +
                        Funcoes.FormatarZerosEsquerda(item.toticms.Value, 13, true) +
                        Funcoes.FormatarZerosEsquerda(0, 13, true) + // Isenta ou nao Tributada
                        Funcoes.FormatarZerosEsquerda(valorOutras, 13, true) +
                        Funcoes.FormatarZerosEsquerda(item.icmsentrada, 4, true) +
                        situacaoNF);
                    contadorReg50++;

                    //Itens              
                    if (!numerosProcessados.Contains((int)item.numero))
                    {
                        var dadosItens = (from n in Conexao.CriarEntidade().registro50entradas_itens
                                          orderby n.numero
                                          where n.numero == item.numero
                                          && n.nf == item.nf
                                          && n.codigofilial == filial
                                          select n);
                        var registro50Itens = dadosItens.ToList();
                        int seq = 0;
                        foreach (var colecaoItens in registro50Itens)
                        {
                            if (!codigos.Contains(colecaoItens.codigo))
                                codigos.Add(colecaoItens.codigo);
                            seq++;
                            reg54Itens.AppendLine("54" +
                                dadoEmitente.CGC.PadRight(14, '0').Substring(0, 14) +
                                infoNF.modeloNF +
                                infoNF.serie.PadRight(3, ' ').Substring(0, 3) +
                                infoNF.NF.PadLeft(6, '0').Substring(0, 6) +
                                colecaoItens.cfopentrada.Replace(".", "").PadLeft(4, '0').Substring(0, 4) +
                                colecaoItens.tributacao +
                                seq.ToString().PadLeft(3, '0') +
                                colecaoItens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                Funcoes.FormatarZerosEsquerda(colecaoItens.quantidade.Value, 11, true, 3) +
                                Funcoes.FormatarZerosEsquerda(colecaoItens.totalNF.Value, 12, true) +
                                Funcoes.FormatarZerosEsquerda(0, 12, true) +
                                Funcoes.FormatarZerosEsquerda(colecaoItens.bcicms.Value, 12, true) +
                                Funcoes.FormatarZerosEsquerda(0, 12, true) +
                                Funcoes.FormatarZerosEsquerda(colecaoItens.ipiItem.Value, 12, true) +
                                Funcoes.FormatarZerosEsquerda(colecaoItens.icmsentrada, 4, true));
                            contadorReg54++;
                        }
                        numerosProcessados.Add((int)item.numero);
                    }

                    //Itens
                }
                catch (Exception ex)
                {
                    listaErros.AppendLine("Erro: Nota de Entrada Nr: " + item.numero + " NF: " + item.nf + "Exceção " + ex.Message);
                }
            }

        }

        public void Registro50Saidas()
        {
            List<string> numerosProcessados = new List<string>();

            var lstNotasFiscal = (from n in Conexao.CriarEntidade().contnfsaida
                                  where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                  && n.codigofilial == filial
                                  && n.exportarfiscal == "S"
                                  && n.finalidade == "1"
                                  orderby n.notafiscal
                                  select n);


            bool processarNFe = true;
            foreach (var itemNotaFiscal in lstNotasFiscal)
            {
                try
                {
                    processarNFe = true;
                    string cnpj = "";
                    string ie = "";
                    string estado = "";

                    //Int64 notaFiscal = Convert.ToInt64(item.notafiscal);
                    //var dadosNF = (from n in entidade.contnfsaida
                    //               where n.notafiscal == notaFiscal && n.serie == item.serienf
                    //               && n.codigofilial == GlbVariaveis.glb_filial
                    //               && n.data == item.DATA
                    //               select n).First();


                    if (itemNotaFiscal.codfornecedor > 0)
                    {

                        var dadosFor = (from n in entidade.fornecedores
                                        where n.Codigo == itemNotaFiscal.codfornecedor
                                        select new { n.CGC, n.INSCRICAO, n.ESTADO, n.CPF }).FirstOrDefault();
                        if (dadosFor == null)
                            processarNFe = false;
                        //throw new Exception("Cod. Forn. " + itemNotaFiscal.codfornecedor + " não encontrado. NF: " + itemNotaFiscal.notafiscal);
                        if (processarNFe)
                        {
                            if (!codigoParticipante.Contains("F" + itemNotaFiscal.codfornecedor.ToString()))
                                codigoParticipante.Add("F" + itemNotaFiscal.codfornecedor.ToString());
                            cnpj = dadosFor.CGC;
                            if (string.IsNullOrEmpty(cnpj))
                                cnpj = dadosFor.CPF;

                            ie = dadosFor.INSCRICAO;
                            estado = dadosFor.ESTADO;
                        }
                    }

                    if (itemNotaFiscal.codcliente > 0)
                    {
                        var dadosCli = (from n in entidade.clientes
                                        where n.Codigo == itemNotaFiscal.codcliente
                                        select new { n.cnpj, n.cpf, n.inscricao, n.estado }).FirstOrDefault();

                        if (dadosCli == null)
                        {
                            processarNFe = false;
                            //throw new Exception("Cod. Forn. " + itemNotaFiscal.codcliente + " não encontrado. NF: " + itemNotaFiscal.notafiscal);
                        }
                        if (processarNFe)
                        {
                            if (!codigoParticipante.Contains("C" + itemNotaFiscal.codcliente.ToString()))
                                codigoParticipante.Add("C" + itemNotaFiscal.codcliente.ToString());


                            cnpj = dadosCli.cnpj;
                            if (string.IsNullOrEmpty(cnpj))
                                cnpj = dadosCli.cpf;

                            ie = dadosCli.inscricao;
                            estado = dadosCli.estado;
                        }
                    }

                    if (string.IsNullOrEmpty(ie))
                        ie = "ISENTO";


                    // Pegando o agrupoamento dos CFOPs e ICMS
                    string nf = itemNotaFiscal.notafiscal.ToString();
                    siceEntities entidadeNotas = Conexao.CriarEntidade();
                    var dados = from n in entidadeNotas.registro50saida_agr
                                where n.notafiscal == nf && n.serienf == itemNotaFiscal.serie
                                && n.codigofilial == filial
                                select n;

                    var registro50 = dados.ToList();
                    decimal? valorOutras = 0;


                    // Somando os totais da Nota Fiscal
                    decimal? totalNF = (from n in registro50
                                        select n.SUM_TOTAL_).Sum().Value;

                    decimal? totalBCIcms = (from n in registro50
                                            select n.baseCalculoICMS).Sum();

                    decimal? totalICMS = (from n in registro50
                                          select n.totalicms).Sum();

                    decimal? totalOutrasAliquotas = (from n in registro50
                                                     where n.baseCalculoICMS == 0
                                                     select (decimal?)n.SUM_TOTAL_).Sum();


                    foreach (var item in registro50)
                    {
                        if (processarNFe)
                        {
                            string situacaoNF = "N";
                            if (itemNotaFiscal.situacaoNF == "02" || itemNotaFiscal.situacaoNF == "S" || itemNotaFiscal.situacaoNF == "03" || itemNotaFiscal.situacaoNF == "04" || itemNotaFiscal.situacaoNF == "05")
                                situacaoNF = "S";

                            valorOutras = 0;
                            if (item.baseCalculoICMS == 0)
                                valorOutras = item.SUM_TOTAL_.Value;

                            conteudo.AppendLine("50" +
                                cnpj.PadLeft(14, '0').Substring(0, 14) +
                                ie.Trim().PadRight(14, ' ').Substring(0, 14) +
                                string.Format("{0:yyyyMMdd}", itemNotaFiscal.dataemissao) +
                                estado +
                                itemNotaFiscal.modelodocfiscal.PadLeft(2, '0').Substring(0, 2) +
                                itemNotaFiscal.serie.PadRight(3, ' ').Substring(0, 3) +
                                Funcoes.FormatarZerosEsquerda(itemNotaFiscal.notafiscal.Value, 6, false) +
                                item.cfop.Replace(".", "").PadLeft(4, '0').Substring(0, 4) +
                                "P" +
                                Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 13, true) +
                                Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value, 13, true) +
                                Funcoes.FormatarZerosEsquerda(item.totalicms.Value, 13, true) +
                                Funcoes.FormatarZerosEsquerda(0, 13, true) +
                                Funcoes.FormatarZerosEsquerda(valorOutras.Value, 13, true) +
                                Funcoes.FormatarZerosEsquerda(item.icms, 4, true) +
                                situacaoNF);
                            contadorReg50++;

                            // Itens saida
                            if (!numerosProcessados.Contains(item.notafiscal))
                            {
                                var dadosItens = (from n in Conexao.CriarEntidade().registro50saidas_itens
                                                  where n.DATA >= dataInicial.Date && n.DATA <= dataFinal
                                                  && n.notafiscal != ""
                                                  && n.notafiscal == item.notafiscal && n.serienf == item.serienf
                                                  && n.codigofilial == filial
                                                  orderby n.codigo
                                                  select n);

                                var registro50Itens = dadosItens.ToList();

                                int seq = 0;
                                foreach (var colecaoItens in registro50Itens)
                                {

                                    if (!codigos.Contains(colecaoItens.codigo))
                                        codigos.Add(colecaoItens.codigo);
                                    seq++;

                                    reg54Itens.AppendLine("54" +
                                        cnpj.PadLeft(14, '0').Substring(0, 14) +
                                        itemNotaFiscal.modelodocfiscal.PadLeft(2, '0') +
                                        itemNotaFiscal.serie.PadRight(3, ' ').Substring(0, 3) +
                                        itemNotaFiscal.notafiscal.ToString().PadLeft(6, '0').Substring(0, 6) +
                                        colecaoItens.cfop.Replace(".", "").PadLeft(4, '0').Substring(0, 4) +
                                        colecaoItens.tributacao.PadLeft(3, '0') +
                                        seq.ToString().PadLeft(3, '0') +
                                        colecaoItens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                        Funcoes.FormatarZerosEsquerda(colecaoItens.SUM_quantidade_.Value, 11, true, 3) +
                                        Funcoes.FormatarZerosEsquerda(colecaoItens.SUM_TOTAL_.Value, 12, true) +
                                        Funcoes.FormatarZerosEsquerda(colecaoItens.descontovalor.Value, 12, true) +
                                        Funcoes.FormatarZerosEsquerda(colecaoItens.baseCalculoICMS.Value, 12, true) +
                                        Funcoes.FormatarZerosEsquerda(0, 12, true) +
                                        Funcoes.FormatarZerosEsquerda(0, 12, true) +
                                        Funcoes.FormatarZerosEsquerda(colecaoItens.icms, 4, true));
                                    contadorReg54++;
                                } //Itens
                                numerosProcessados.Add(item.notafiscal);
                            }
                        } // If processarNF
                    } // foreach 50

                }
                catch
                {
                    throw new Exception("erro. Entrada Nr.:" + itemNotaFiscal.numero.ToString() + "NFe :" + itemNotaFiscal.notafiscal);
                }

            };

        }

        public void Registro60()
        {
            var dados60M = from n in entidade.C60m
                           where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                           orderby n.ECFnumeroserie
                           select new
                           {
                               n.data,
                               n.ECFnumeroserie,
                               n.ECFnumero,
                               n.modeloDocFiscal,
                               n.contadorinicial,
                               n.contadorfinal,
                               n.numeroreducaoZ,
                               n.contadorreinicio,
                               n.vendabruta,
                               n.vendaliquida,
                               n.totalgeralECF,
                               n.hora
                           };
            var registro60M = dados60M.ToList().Distinct();

            foreach (var r60M in registro60M)
            {
                conteudo.AppendLine("60" +
                    "M" +
                     string.Format("{0:yyyyMMdd}", r60M.data) +
                     r60M.ECFnumeroserie.PadRight(20, ' ').Substring(0, 20) +
                     r60M.ECFnumero.PadLeft(3, '0') +
                     r60M.modeloDocFiscal +
                     r60M.contadorinicial.PadLeft(6, '0') +
                     r60M.contadorfinal.PadLeft(6, '0') +
                     r60M.numeroreducaoZ.PadLeft(6, '0') +
                     r60M.contadorreinicio.PadLeft(4, '0').Substring(1, 3) +
                     Funcoes.FormatarZerosEsquerda(r60M.vendabruta, 16, true) +
                     Funcoes.FormatarZerosEsquerda(r60M.totalgeralECF, 16, true) +
                     " ".PadRight(37, ' ').Substring(0, 37));
                contadorReg60++;


                var dados60 = from n in entidade.C60a
                              where n.data == r60M.data
                              && n.ECFnumeroserie == r60M.ECFnumeroserie
                              && n.acumuladoTotalizadorParcial > 0
                              && n.hora == r60M.hora
                              select new { n.data, n.hora, n.aliquotaICMS, n.ECFnumeroserie, n.acumuladoTotalizadorParcial };
                var registro60 = dados60.ToList().Distinct();

                foreach (var item in registro60)
                {
                    conteudo.AppendLine("60" +
                        "A" +
                        string.Format("{0:yyyyMMdd}", item.data) +
                        item.ECFnumeroserie.PadRight(20, ' ').Substring(0, 20) +
                        item.aliquotaICMS.PadRight(4, ' ') +
                        Funcoes.FormatarZerosEsquerda(item.acumuladoTotalizadorParcial, 12, true) +
                        " ".PadRight(79, ' ').Substring(0, 79));
                    contadorReg60++;
                }

                var dados60D = from n in entidade.C60d
                               where n.DATA == r60M.data
                               && n.ecffabricacao == r60M.ECFnumeroserie
                               select n;

                var registro60D = dados60D.ToList();

                foreach (var item in registro60D)
                {

                    if (!codigos.Contains(item.codigo))
                        codigos.Add(item.codigo);


                    var baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value - item.descontovalorCupom.Value, 16, true);
                    decimal? totalICMS = (item.baseCalculoICMS.Value - item.descontovalorCupom) * item.icms / 100;
                    totalICMS = (Math.Truncate(totalICMS * 100 ?? 0) / 100);

                    var aliquota = Funcoes.FormatarZerosEsquerda(item.icms, 4, true);
                    if (item.tributacao == "60")
                    {
                        aliquota = "F   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "00" && item.icms == 0 || item.tributacao == "40" || item.tributacao == "41")
                    {
                        aliquota = "I   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "80")
                    {
                        aliquota = "N   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    conteudo.AppendLine("60" +
                        "D" +
                        string.Format("{0:yyyyMMdd}", item.DATA) +
                        item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                        item.codigo.PadRight(14, ' ').Substring(0, 14) +
                        Funcoes.FormatarZerosEsquerda(item.SUM_quantidade_.Value, 13, true, 3) +
                        Funcoes.FormatarZerosEsquerda((item.SUM_TOTAL_.Value + item.descontovalor.Value) - item.acrescimototalitem.Value, 16, true) +
                        baseCalculo +
                        aliquota.ToString() +
                        Funcoes.FormatarZerosEsquerda(totalICMS ?? 0, 13, true) +
                        " ".PadRight(19, ' ').Substring(0, 19));
                    contadorReg60++;
                }

                var dados60I = from n in entidade.C60i
                               where n.DATA == r60M.data
                                && n.ecffabricacao == r60M.ECFnumeroserie
                                && n.SUM_quantidade_ > 0
                               select n;

                var registro60I = dados60I.ToList();

                foreach (var item in registro60I)
                {
                    if (!codigos.Contains(item.codigo))
                        codigos.Add(item.codigo);
                    var baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value - item.descontovalorCupom.Value, 12, true);
                    decimal? totalICMS = (item.baseCalculoICMS.Value - item.descontovalorCupom) * item.icms / 100;
                    totalICMS = (Math.Truncate(totalICMS * 100 ?? 0) / 100);

                    var aliquota = Funcoes.FormatarZerosEsquerda(item.icms, 4, true);
                    if (item.tributacao == "60")
                    {
                        aliquota = "F   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 12, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "00" && item.icms == 0 || item.tributacao == "40" || item.tributacao == "41")
                    {
                        aliquota = "I   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 12, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "80")
                    {
                        aliquota = "N   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 12, true);
                        totalICMS = item.totalicms;
                    }
                    conteudo.AppendLine("60" +
                     "I" +
                     string.Format("{0:yyyyMMdd}", item.DATA) +
                     item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                     "2D" +
                     item.coo.PadRight(6, '0').Substring(0, 6) +
                     item.nrcontrole.ToString().PadLeft(3, '0') +
                     item.codigo.PadRight(14, ' ').Substring(0, 14) +
                     Funcoes.FormatarZerosEsquerda(item.SUM_quantidade_.Value, 13, true, 3) +
                     Funcoes.FormatarZerosEsquerda(item.SUM_TOTAL_.Value, 13, true, 3) +
                     baseCalculo +
                     aliquota +
                     Funcoes.FormatarZerosEsquerda(totalICMS.Value, 12, true) +
                     " ".PadRight(16, ' ').Substring(0, 16));
                    contadorReg60++;
                }

                var dados60r = from n in entidade.C60r
                               where n.DATA == r60M.data
                                && n.ecffabricacao == r60M.ECFnumeroserie
                               select n;

                var registro60R = dados60r.ToList();

                foreach (var item in registro60R)
                {

                    var baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value - item.descontovalor.Value, 16, true);
                    decimal? totalICMS = (item.baseCalculoICMS.Value - item.descontovalor) * item.icms / 100;
                    totalICMS = (Math.Truncate(totalICMS * 100 ?? 0) / 100);

                    var aliquota = Funcoes.FormatarZerosEsquerda(item.icms, 4, true);
                    if (item.tributacao == "60")
                    {
                        aliquota = "F   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "00" && item.icms == 0 || item.tributacao == "40" || item.tributacao == "41")
                    {
                        aliquota = "I   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    if (item.tributacao == "80")
                    {
                        aliquota = "N   ";
                        baseCalculo = Funcoes.FormatarZerosEsquerda(item.baseCalculoICMS.Value, 16, true);
                        totalICMS = item.totalicms;
                    }

                    conteudo.AppendLine("60" +
                    "R" +
                    string.Format("{0:MMyyyy}", item.DATA) +
                    item.codigo.PadRight(14, ' ').Substring(0, 14) +
                    Funcoes.FormatarZerosEsquerda(item.SUM_quantidade_.Value, 13, true, 3) +
                    Funcoes.FormatarZerosEsquerda(item.preco.Value, 16, true) +
                    baseCalculo +
                    aliquota +
                    " ".PadRight(54, ' ').Substring(0, 54));
                    contadorReg60++;
                }

            };

        }

        private void Registro61()
        {

            var mestre = from n in entidade.contdocs
                         where n.data >= dataInicial && n.data <= dataFinal.Date
                         && (n.modeloDOCFiscal == "02" || n.modeloDOCFiscal == "D1" || n.numeroPED>0)
                         select n;
            if (mestre.Count() == 0)
                return;

            var registro61 = mestre.ToList();

            Int32? numeroFinal = (from n in mestre
                                  select (Int32?)n.documento).Max();

            Int32? numeroInicial = (from n in mestre
                                    select (Int32?)n.documento).Min();

            foreach (var itemMestre in registro61)
            {
                var dadosItens = from n in entidade.blococregc390
                                 where n.documento == itemMestre.documento
                                 select n;

                foreach (var item in dadosItens)
                {
                    string modDoc = itemMestre.modeloDOCFiscal;
                    if (item.notafiscal != null)
                        modDoc = "02";

                    conteudo.AppendLine("61" + //01
                        " ".PadRight(14, ' ').Substring(0, 14) +
                        " ".PadRight(14, ' ').Substring(0, 14) +
                        string.Format("{0:yyyyMMdd}", item.data) +
                         modDoc.Trim().PadRight(2, ' ').Substring(0, 2) +
                         "D  " +// itemMestre.serienf.Trim().PadRight(3, ' ').Substring(0, 3) +
                         itemMestre.subserienf.Trim().PadRight(2, ' ').Substring(0, 2) +
                         numeroInicial.ToString().PadRight(6, '0').Substring(0, 6) +
                         numeroFinal.ToString().PadRight(6, '0').Substring(0, 6) +
                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 13, true) +
                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.baseCalculoICMS), 13, true) +
                         Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.totalICMS), 12, true) +
                         Funcoes.FormatarZerosEsquerda(0, 13, true) +
                         Funcoes.FormatarZerosEsquerda(0, 13, true) +
                         Funcoes.FormatarZerosEsquerda(item.icms, 4, true) +
                         " ");
                    contadorReg61++;
                };
            };


            var resumo = from n in entidade.blococregc425
                         where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                         && (n.modelodocfiscal == "D1" || n.modelodocfiscal == "02")
                         select n;
            foreach (var item in resumo)
            {
                if (!codigos.Contains(item.codigo))
                    codigos.Add(item.codigo);

                conteudo.AppendLine("61" +
                    "R" +
                    string.Format("{0:MMyyyy}", item.data) +
                    item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.quantidade), 13, true) +
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 16, true) +
                    Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.total), 16, true) +
                    Funcoes.FormatarZerosEsquerda(item.icms, 4, true) +
                    " ".PadRight(54, ' ').Substring(0, 54));
                contadorReg61++;
            }

        }

        public void Registro74()
        {
            StringBuilder conteudo74 = new StringBuilder();
            if (numeroInventario == 0)
                return;
            var obterReg74 = from n in entidade.produtosinventario
                             where n.quantidade > 0 && n.codigo != ""
                             && n.customedio > 0
                             && n.inventarionumero == numeroInventario
                             && n.anoinventario == anoInventario
                             select new { n.codigo, n.unidade, n.quantidade, n.customedio };

            var dadosReg74 = obterReg74.ToList();

            var reg74 = from p in dadosReg74
                        group p by new { codigo = p.codigo, custo = p.customedio, unidade = p.unidade } into g
                        select new { g.Key.codigo, quantidade = g.Sum(p => p.quantidade), total = g.Sum(p => p.quantidade * p.customedio) };


            foreach (var item in reg74)
            {
                if (!conteudo74.ToString().Contains(item.codigo))
                {
                    codigos.Add(item.codigo);
                    conteudo74.AppendLine("74" +
                        string.Format("{0:yyyyMMdd}", dataFinal) +
                        item.codigo.PadRight(14, ' ').Substring(0, 14) +
                        Funcoes.FormatarZerosEsquerda(item.quantidade, 13, true, 3) +
                        Funcoes.FormatarZerosEsquerda(item.total.Value, 13, true, 2) +
                        "1" +
                        "00000000000000" +
                        new string(' ', 14) +
                        "  " +
                        new string(' ', 45));
                    contadorReg74++;
                }
            }
            conteudo.Append(conteudo74);
        }


        public void Registro75()
        {
            Produtos dadosItem = new Produtos();
            foreach (var item in codigos)
            {
                try
                {
                    dadosItem.ProcurarCodigo(item, GlbVariaveis.glb_filial, false);
                }
                catch
                {
                    try
                    {
                        using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                        {
                            conn.Open();
                            EntityCommand cmd = conn.CreateCommand();
                            cmd.CommandTimeout = 3600;
                            cmd.CommandText = "siceEntities.recriarCodigoProduto";
                            cmd.CommandType = CommandType.StoredProcedure;

                            EntityParameter tabela = cmd.Parameters.Add("filial", DbType.String);
                            tabela.Direction = ParameterDirection.Input;
                            tabela.Value = GlbVariaveis.glb_filial;

                            EntityParameter parFilial = cmd.Parameters.Add("novoCodigo", DbType.String);
                            parFilial.Direction = ParameterDirection.Input;
                            parFilial.Value = item;

                            cmd.ExecuteNonQuery();
                            conn.Close();
                            dadosItem.ProcurarCodigo(item, GlbVariaveis.glb_filial, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exceção ao processar SP recriarCodigoProduto " + ex.Message);
                    }

                }

                conteudo.AppendLine("75" +
                                    string.Format("{0:yyyyMMdd}", dataInicial.Date) +
                                    string.Format("{0:yyyyMMdd}", dataFinal.Date) +
                                    dadosItem.codigo.PadRight(14, ' ').Substring(0, 14) +
                                    dadosItem.ncm.PadRight(8, ' ').Substring(0, 8) +
                                    dadosItem.descricao.Trim().PadRight(53, ' ').Substring(0, 53) +
                                    dadosItem.unidade.PadRight(6, ' ').Substring(0, 6) +
                                    dadosItem.tributacao.PadLeft(3, '0').Substring(0, 3) +
                                    Funcoes.FormatarZerosEsquerda(dadosItem.ipi, 4, false) +
                                    Funcoes.FormatarZerosEsquerda(dadosItem.icms, 4, false) +
                                    Funcoes.FormatarZerosEsquerda(dadosItem.reducaoBaseCalcICMS, 4, true) +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true));
                contadorReg75++;
            }
        }

        public void Registro90()
        {
            int contadorRegistro = contadorReg10 + contadorReg11 + contadorReg50 + contadorReg54 + contadorReg60 + contadorReg61 + contadorReg74 + contadorReg75 + 1;

            conteudo.AppendLine("90" +
                cnpj.PadLeft(14, ' ').Substring(0, 14) +
                ie.PadRight(14, ' ').Substring(0, 14) +
                "50" + contadorReg50.ToString().PadLeft(8, '0') +
                "54" + contadorReg54.ToString().PadLeft(8, '0') +
                "60" + contadorReg60.ToString().PadLeft(8, '0') +
                "61" + contadorReg61.ToString().PadLeft(8, '0') +
                "74" + contadorReg74.ToString().PadLeft(8, '0') +
                "75" + contadorReg75.ToString().PadLeft(8, '0') +
                "99" + contadorRegistro.ToString().PadLeft(8, '0') +
                " ".PadRight(25, ' ') + "1"
                );
            //string nomeArquivoDestino = @"C:\iqsistemas\sice.net\exportacao\SEF\"+nomeArquivo; //  @ConfigurationManager.AppSettings["dirMovimentoECF"] + @"\Entradas.txt";
            using (FileStream fs = File.Create(@nomeArquivo))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(@conteudo);
                }
            };
            System.Threading.Thread.Sleep(1500);
        }


        public static List<LayoutSintegra> EstruturaLayoutSintegra()
        {
            var dados = new[] 
            {
              new LayoutSintegra {codigo=1,descricao="1 - Estrutura conforme Convênio ICMS 57/95 na versão do convêncio ICMS 31/99"},
              new LayoutSintegra {codigo=2,descricao="2 - Estrutura conforme Convênio ICMS 57/95 na versão atual"}
            };
            return dados.ToList();
        }

        public static List<FinalidadeSintegra> FinalidadeArquivoSintegra()
        {
            var dados = new[] 
            {
              new FinalidadeSintegra {codigo=1,descricao="1 - Normal"},
              new FinalidadeSintegra {codigo=2,descricao="2 - Retificação Total do Arquivo."},
              new FinalidadeSintegra {codigo=3,descricao="3 - Retificação aditiva do arquivo."},
              new FinalidadeSintegra {codigo=5,descricao="5 - Desfazimento."}
            };
            return dados.ToList();
        }

        public static List<NaturezaOperacaoSintegra> NaturezaOperSintegra()
        {
            var dados = new[] 
            {
              new NaturezaOperacaoSintegra {codigo=1,descricao="1 - Interestaduais somente ST"},
              new NaturezaOperacaoSintegra {codigo=2,descricao="2 - Interestaduais com ou sem ST"},
              new NaturezaOperacaoSintegra {codigo=3,descricao="3 - Totalidade das oper do informante"},              
            };
            return dados.ToList();
        }

        public struct LayoutSintegra
        {
            public int codigo { get; set; }
            public string descricao { get; set; }
        }

        public struct FinalidadeSintegra
        {
            public int codigo { get; set; }
            public string descricao { get; set; }
        }
        public struct NaturezaOperacaoSintegra
        {
            public int codigo { get; set; }
            public string descricao { get; set; }
        }
    }
}
