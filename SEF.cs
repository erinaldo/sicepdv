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

    class SEF : dadosFiscaisEmpresa
    {
        public int codigoLayout { get; set; }
        public int codigoNaturezaOperacao { get; set; }
        public int codigoFinalidade { get; set; }
        public string filial { get; set; }
        public int numeroInventario { get; set; }
        public int anoInventario { get; set; }
        public string nomeArquivo = "SEF.txt";

        private int contadorReg10 = 1;
        private int contadorReg11 = 1;
        private int contadorReg50 = 0;
        private int contadorReg60 = 0;
        private int contadorReg61 = 0;
        private int contadorReg54 = 0;
        private int contadorReg74 = 0;
        private int contadorReg75 = 0;
        private int contadorReg88 = 0;

        private List<string> codigos = new List<string>();
        List<string> codigoParticipante = new List<string>();
        public StringBuilder listaErros = new StringBuilder();

        public SEF()
        {
            codigoLayout = 2;
            codigoNaturezaOperacao = 1;
            codigoFinalidade = 1;
            filial = GlbVariaveis.glb_filial;
            ObterDados();
            Funcoes.ProcedureAjuste("AjustarCamposNulos");
            Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);

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
            contato = Funcoes.RetirarAcentos(dados.responsavel);
            responsavel = Funcoes.RetirarAcentos(dados.responsavel);
            cpfresponsavel = dados.cpfresponsavel;
            // Contador
            nomeContador = Funcoes.RetirarAcentos(dados.contador);
            cpfContador = dados.cpfcontador ?? "";
            CRC = dados.crccontador;
            cnpjContador = dados.cnpjcontador;
            cepContador = dados.cepcontador;
            endContador = dados.enderecocontador;
            EndNumeroContador = dados.numerocontador;
            complementoContador = dados.complementocontador;
            bairroContador = dados.bairrocontador;
            foneContador = dados.telefonecontador ?? "";
            faxContador = dados.faxcontador ?? "";
            emailContador = dados.emailcontador ?? "" ;

            ind_ativ = dados.indicadoratividade.Substring(0, 1);
        }

        public bool GerarEntradas()
        {
            try
            {
                nomeArquivo = "SEF_Entradas.txt";
                Registro10();
                Registro11();
                Registro50Entrada();
                Registro75();

                Registro88();
                
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
                nomeArquivo = "SEF_Saidas.txt";
                Registro10();
                Registro11();
                Registro50Saidas();
                Registro75();
                Registro88();
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
                nomeArquivo = "SEF_Mapa Resumo.txt";
                Registro10();
                Registro11();
                Registro60();
                //Registro61();
                Registro75();
                Registro88();
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
                nomeArquivo = "SEF_Iventario.txt";
                Registro10();
                Registro11();
                Registro74();
                Registro75();
                Registro88();
                Registro90();
                return true;
            }
            catch (Exception erro)
            {

                throw new Exception("Gerando inventário: " + erro.Message);
            }
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
                codigoLayout.ToString() + // Estrutura conforme Convenio ICMS 57/95 cod_ver+
                codigoNaturezaOperacao.ToString() +
                codigoFinalidade.ToString()+
                "1");
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
                         where n.dataentrada >= dataInicial.Date && n.dataentrada <= dataFinal.Date                         
                         && n.codigofilial == filial && n.lancada=="X"                         
                         && n.nf != ""
                         && (n.modelonf.Contains("55") || (n.modelonf.Contains("01")
                         || n.modelonf.Contains("06")
                         || n.modelonf.Contains("21")
                         || n.modelonf.Contains("22")))                         
                         orderby n.numero
                         select n);
            var registro50 = dados.ToList();

            StringBuilder itens = new StringBuilder();
            List<int> numerosProcessados = new List<int>();
            foreach (var item in registro50)
            {

                try
                {
                    
                    var dadoNF = (from n in entidade.moventradas
                                  where n.numero == item.numero
                                  && n.NF == item.nf
                                  && n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal.Date
                                  select new {n.fornecedor, n.codigofornecedor,n.situacaoNF,n.NF,n.DataEmissao,n.modeloNF,n.Emitente,n.serie,n.dataEntrada }).FirstOrDefault();

                    if (!codigoParticipante.Contains("F" + dadoNF.codigofornecedor.ToString()))
                    {
                        if (dadoNF.codigofornecedor.ToString() == "0")
                        {
                            var codFor = (from n in Conexao.CriarEntidade().fornecedores
                                          where n.fornecedor == dadoNF.fornecedor
                                          select n.Codigo).FirstOrDefault();
                            codigoParticipante.Add("F" + codFor.ToString());
                        }
                        else
                        {
                            codigoParticipante.Add("F" + dadoNF.codigofornecedor.ToString());
                        }
                    }

                    
                var infoNF = dadoNF;

                var dadoEmitente = (from n in entidade.fornecedores
                                    where n.Codigo == infoNF.codigofornecedor
                                    select new { n.Codigo, n.empresa,n.CGC,n.CPF,n.ESTADO,n.INSCRICAO }).FirstOrDefault();
                var infoEmitente = dadoEmitente;                

                // Outras - Valor que não confira débito ou crédito do ICMS

                decimal valorOutras = 0;
                if (item.bcicms==0)                
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
                                                 && n.bcicms==0
                                                 select (decimal?)n.totalNF).Sum();

                if (!totalOutrasAliquotas.HasValue)
                    totalOutrasAliquotas = 0;

                string ieRegistro = infoEmitente.INSCRICAO;                
                if (string.IsNullOrEmpty(ieRegistro))
                    ieRegistro="ISENTO";

                string situacaoNF = "N";
                if (infoNF.situacaoNF == "02" || infoNF.NF=="S" || infoNF.situacaoNF =="03" || infoNF.situacaoNF=="04" || infoNF.situacaoNF=="05")
                    situacaoNF = "S";

                conteudo.AppendLine("50" +
                    (infoEmitente.CGC + infoEmitente.CPF).PadRight(14, ' ').Substring(0, 14) +
                    ieRegistro.PadRight(14, ' ').Substring(0, 14) +
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
                    situacaoNF+
                    string.Format("{0:yyyyMMdd}", dadoNF.DataEmissao) +
                    "000000000000" +
                    "000"+
                    new string(' ',9) + //CODUNC
                    "00"+// 22- Código de classe de consumo de energia elétrica
                    "00000000"+
                    "00000000"+
                    "0000000"+
                    "0000000"+
                    "000000000"+
                    Funcoes.FormatarZerosEsquerda(totalNF.Value, 13, true) +
                    Funcoes.FormatarZerosEsquerda(totalBCIcms.Value, 13, true) +
                    Funcoes.FormatarZerosEsquerda(totalICMS.Value, 13, true) +
                    Funcoes.FormatarZerosEsquerda(0, 13, true) +//31 Isenção
                    Funcoes.FormatarZerosEsquerda(totalOutrasAliquotas.Value, 13, true)+
                    "00000");
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
                        itens.AppendLine("54" +
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
                            Funcoes.FormatarZerosEsquerda(colecaoItens.icmsentrada, 4, true) +
                            ieRegistro.PadRight(14, ' ').Substring(0, 14) +
                            string.Format("{0:yyyyMMdd}", dadoNF.dataEntrada) +
                            dadoEmitente.ESTADO +
                            "00" +
                            "01");
                        contadorReg54++;
                    }
                    numerosProcessados.Add((int)item.numero);
                }//Itens

                }
                catch (Exception ex)
                {
                    listaErros.AppendLine("Erro: Nota de Entrada Nr: " + item.numero + " NF: " + item.nf + "Exceção " + ex.Message);
                }
            }
            conteudo.Append(itens);
        }        

        public void Registro50Saidas()
        {
            StringBuilder itens = new StringBuilder();
            List<string> numerosProcessados = new List<string>();            

            var lstNotasFiscal = (from n in Conexao.CriarEntidade().contnfsaida
                                  where n.dataemissao >= dataInicial && n.dataemissao <= dataFinal
                                  && n.codigofilial == filial
                                  && n.finalidade == "1"
                                  && n.exportarfiscal=="S"           
                                  orderby n.notafiscal
                                  select n);
            
            foreach (var itemNotaFiscal in lstNotasFiscal)
            {

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
                    if (!codigoParticipante.Contains("F" + itemNotaFiscal.codfornecedor.ToString()))
                        codigoParticipante.Add("F" + itemNotaFiscal.codfornecedor.ToString());

                    var dadosFor = (from n in entidade.fornecedores
                                    where n.Codigo == itemNotaFiscal.codfornecedor
                                    select new { n.CGC, n.INSCRICAO, n.ESTADO, n.CPF }).First();
                    if (dadosFor==null)
                        throw new Exception("Cod. Forn. "+itemNotaFiscal.codfornecedor+" não encontrado. NF: "+itemNotaFiscal.notafiscal);

                    cnpj = dadosFor.CGC;
                    if (string.IsNullOrWhiteSpace(cnpj))
                        cnpj = dadosFor.CPF;

                    ie = dadosFor.INSCRICAO;
                    estado = dadosFor.ESTADO;
                }

                if (itemNotaFiscal.codcliente > 0)
                {
                    if (!codigoParticipante.Contains("C" + itemNotaFiscal.codcliente.ToString()))
                        codigoParticipante.Add("C" + itemNotaFiscal.codcliente.ToString());

                    var dadosCli = (from n in entidade.clientes
                                    where n.Codigo == itemNotaFiscal.codcliente
                                    select new { n.cnpj, n.cpf, n.inscricao, n.estado }).First();

                    if (dadosCli == null)
                        throw new Exception("Cod. Cli. " + itemNotaFiscal.codcliente + " não encontrado. NF: " + itemNotaFiscal.notafiscal);

                    cnpj = dadosCli.cnpj;
                    if (string.IsNullOrWhiteSpace(cnpj))
                        cnpj = dadosCli.cpf;

                    ie = dadosCli.inscricao;
                    estado = dadosCli.estado;
                }

                if (string.IsNullOrEmpty(ie))
                    ie = "ISENTO";


                // Pegando o agrupoamento dos CFOPs e ICMS
                string nf = itemNotaFiscal.notafiscal.ToString();
                siceEntities entidadeNotas = Conexao.CriarEntidade();
                var dados = from n in entidadeNotas.registro50saida_agr
                            where n.notafiscal==nf && n.serienf == itemNotaFiscal.serie
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

                    var dadosItens = (from n in Conexao.CriarEntidade().registro50saidas_itens
                                      where n.DATA >= dataInicial.Date && n.DATA <= dataFinal
                                      && n.notafiscal != ""
                                      && n.notafiscal == item.notafiscal && n.serienf == item.serienf
                                      && n.codigofilial == filial
                                      orderby n.codigo
                                      select n);

                    if (dadosItens.Count() > 0)
                    {
                        valorOutras = 0;
                        if (item.baseCalculoICMS == 0)
                            valorOutras = item.SUM_TOTAL_.Value;

                        string situacaoNF = "N";
                        if (itemNotaFiscal.situacaoNF == "02" || itemNotaFiscal.situacaoNF == "S" || itemNotaFiscal.situacaoNF == "03" || itemNotaFiscal.situacaoNF == "04" || itemNotaFiscal.situacaoNF == "05")
                            situacaoNF = "S";


                        conteudo.AppendLine("50" +
                            cnpj.PadLeft(14, '0').Substring(0, 14) +
                            ie.Replace(".", "").Replace("-", "").PadRight(14, ' ').Substring(0, 14) +
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
                            situacaoNF +
                            string.Format("{0:yyyyMMdd}", itemNotaFiscal.dataemissao) +
                            "000000000000" +
                            "000" +
                            new string(' ', 9) + //CODUNC
                            "00" +// 22- Código de classe de consumo de energia elétrica
                            "00000000" +
                            "00000000" +
                            "0000000" +
                            "0000000" +
                            "000000000" +
                            Funcoes.FormatarZerosEsquerda(totalNF.Value, 13, true) +
                            Funcoes.FormatarZerosEsquerda(totalBCIcms.Value, 13, true) +
                            Funcoes.FormatarZerosEsquerda(totalICMS.Value, 13, true) +
                            Funcoes.FormatarZerosEsquerda(0, 13, true) +//31 Isenção
                            Funcoes.FormatarZerosEsquerda(totalOutrasAliquotas.Value, 13, true) +
                            "00000");
                        contadorReg50++;

                        // Itens saida
                        if (!numerosProcessados.Contains(item.notafiscal))
                        {


                            var registro50Itens = dadosItens.ToList();

                            int seq = 0;
                            foreach (var colecaoItens in registro50Itens)
                            {



                                if (!codigos.Contains(colecaoItens.codigo))
                                    codigos.Add(colecaoItens.codigo);
                                seq++;

                                itens.AppendLine("54" +
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
                                    Funcoes.FormatarZerosEsquerda(colecaoItens.icms, 4, true) +
                                    ie.PadRight(14, ' ').Substring(0, 14) +
                                    string.Format("{0:yyyyMMdd}", itemNotaFiscal.dataemissao) +
                                    estado +
                                    "00" +
                                    "01");
                                contadorReg54++;
                            } //Itens
                            numerosProcessados.Add(item.notafiscal);
                        }
                    }
                }
            };
            conteudo.Append(itens);
        }
       
        public void Registro60()
        {
            // Criar a tabela temporaria das venda de vendaarquivo para não demorar muito o processamento

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
                               n.hora,
                               n.gtinicialdia,
                               n.ValorICMS
                           };
            if (dados60M == null)
                throw new Exception("Sem movimento");

            var registro60M = dados60M.ToList().Distinct();

            foreach (var r60M in registro60M)
            {
                conteudo.AppendLine("60" +
                    "M" +
                     string.Format("{0:yyyyMMdd}", r60M.data) +
                     r60M.ECFnumeroserie.PadRight(20, ' ').Substring(0, 20) +
                     r60M.ECFnumero.PadLeft(3, '0') +
                     r60M.modeloDocFiscal.PadLeft(2,'0').Substring(0,2) +
                     r60M.contadorinicial.PadLeft(6, '0') +
                     r60M.contadorfinal.PadLeft(6, '0') +
                     r60M.numeroreducaoZ.PadLeft(6, '0') +
                     r60M.contadorreinicio.PadLeft(3, '0').Substring(0, 3) +
                     Funcoes.FormatarZerosEsquerda(r60M.vendabruta, 16, true) +
                     Funcoes.FormatarZerosEsquerda(r60M.totalgeralECF, 16, true) +
                     " ".PadRight(37, ' ').Substring(0, 37)+
                     Funcoes.FormatarZerosEsquerda(r60M.gtinicialdia, 13, true) +
                     Funcoes.FormatarZerosEsquerda(r60M.vendaliquida, 13, true) +
                     Funcoes.FormatarZerosEsquerda(r60M.ValorICMS, 13, true) +
                     "00000");
                contadorReg60++;


                var dados60 = from n in entidade.C60a
                              where n.data == r60M.data
                              && n.ECFnumeroserie == r60M.ECFnumeroserie
                              && n.acumuladoTotalizadorParcial > 0
                              && n.hora == r60M.hora
                              select n;
                var registro60 = dados60.ToList();

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

                    if (item.tributacao == "00" && item.icms == 0)
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

            };

        }

        private void Registro61()
        {

            var mestre = from n in entidade.contdocs
                         where n.data >= dataInicial && n.data <= dataFinal.Date
                         && (n.modeloDOCFiscal == "02" || n.modeloDOCFiscal == "D1")
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
                    conteudo.AppendLine("61" + //01
                        " ".PadRight(14, ' ').Substring(0, 14) +
                        " ".PadRight(14, ' ').Substring(0, 14) +
                        string.Format("{0:yyyyMMdd}", item.data) +
                         itemMestre.modeloDOCFiscal.Trim().PadRight(2, ' ').Substring(0, 2) +
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
            if (numeroInventario == 0)
                return;
            var obterReg74 = from n in entidade.produtosinventario
                          where n.quantidade > 0 && n.codigo != ""
                          && n.customedio>0
                          && n.inventarionumero == numeroInventario
                          && n.anoinventario == anoInventario
                          select new { n.codigo, n.unidade, n.quantidade, n.customedio };

            var Reg74 = obterReg74.ToList().Distinct();
            foreach (var item in Reg74)
            {
                if (!codigos.Contains(item.codigo))
                    codigos.Add(item.codigo);
                conteudo.AppendLine("74" +
                    string.Format("{0:MMyyyy}", dataFinal) +
                    item.codigo.PadRight(14, ' ').Substring(0, 14) +
                    Funcoes.FormatarZerosEsquerda(item.quantidade, 13, true, 3) +
                    Funcoes.FormatarZerosEsquerda(item.quantidade * item.customedio, 13, true, 2) +
                    "1" +
                    "00000000000000" +
                    new string(' ', 14) +
                    " " +
                    new string(' ', 45) +
                    "1" +
                    Funcoes.FormatarZerosEsquerda(item.customedio, 13, true) +
                    "00000");
                contadorReg74++;
            }
        }

        public void Registro75()
        {
            Produtos dados = new Produtos();

            foreach (var item in codigos)
            {
                dados.ProcurarCodigo(item, GlbVariaveis.glb_filial,false);
                
                conteudo.AppendLine("75" +
                                    string.Format("{0:yyyyMMdd}", dataInicial.Date) +
                                    string.Format("{0:yyyyMMdd}", dataFinal.Date) +
                                    dados.codigo.PadRight(14, ' ').Substring(0, 14) +
                                    dados.ncm.PadRight(8, ' ').Substring(0, 8) +
                                    dados.descricao.PadRight(53, ' ').Substring(0, 53) +
                                    dados.unidade.PadRight(6, ' ').Substring(0, 6) +
                                    dados.tributacao.PadLeft(3, '0').Substring(0, 3) +
                                    Funcoes.FormatarZerosEsquerda(dados.ipi, 4, false) +
                                    Funcoes.FormatarZerosEsquerda(dados.icms, 4, false) +
                                    Funcoes.FormatarZerosEsquerda(dados.reducaoBaseCalcICMS, 4, true) +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true)+
                                    new string(' ',14)+
                                    "000000");
                contadorReg75++;
            }
        }

        public void Registro88()
        {

            conteudo.AppendLine("88" +
                (contadorReg88 + 1).ToString().PadLeft(9, '0')+
                "10"+
                "00"+
                string.Format("{0:yyyyMM}", dataFinal.Date) +
                "N"+
                "S"+
                "N");
            contadorReg88++;

            conteudo.AppendLine("88" +
                (contadorReg88 + 1).ToString().PadLeft(9, '0') +
                "10" +
                "10" +
                nomeEmpresa.PadRight(35, ' ').Substring(0, 35) +
                ie.PadRight(14, ' ').Substring(0, 14) +
                cnpj.PadRight(14, '0').Substring(0, 14) +
                fone.PadLeft(12, '0').Substring(0, 12) +
                fax.PadLeft(12, '0').Substring(0, 12) +
                new string(' ', 6) +
                cep.PadRight(8, ' ').Substring(0, 8) +
                email.PadRight(50, ' ').Substring(0, 50));
            contadorReg88++;

              conteudo.AppendLine("88" +
                (contadorReg88 + 1).ToString().PadLeft(9, '0') +
                "10" +
                "20" +
                responsavel.PadRight(40,' ').Substring(0,40)+
                cpfresponsavel.PadLeft(11,'0').Substring(0,11)+
                fone.PadLeft(12, '0').Substring(0, 12) +
                email.PadRight(50, ' ').Substring(0, 50));
             contadorReg88++;

              conteudo.AppendLine("88" +
                (contadorReg88 + 1).ToString().PadLeft(9, '0') +
                "10" +
                "30" +
                nomeContador.PadRight(50, ' ').Substring(0, 50)+
                cpfContador.PadLeft(11,'0').Substring(0,11)+
                CRC.PadLeft(10,'0').Substring(0,10)+
                foneContador.PadLeft(12, '0').Substring(0, 12) +
                fax.PadLeft(12, '0').Substring(0, 12) +
                faxContador.PadLeft(12, '0').Substring(0, 12) +
                new string(' ', 6) +
                emailContador.PadRight(50, ' ').Substring(0, 50));
              contadorReg88++;
            // Codigo Participante

              StringBuilder conteudo88_1040 = new StringBuilder();
              foreach (var item in codigoParticipante)
              {
                  string codigo = "";
                  string nome = "";
                  string cnpjCli = "";
                  string cpfCli = "";
                  string ieCli = "";
                  string enderecoCli = "";
                  string numeroCli = "";
                  string complementoCli = "";
                  string bairroCli = "";
                  string cidadeCli = "";
                  string estadoCli = "";
                  string tipo="1";

                  if (item.Substring(0, 1) == "F")
                  {
                      Int32 codFor = Convert.ToInt32(item.Substring(1, item.Length - 1));

                      var dados = (from n in entidade.fornecedores
                                   where n.Codigo == codFor
                                   select new
                                   {
                                       n.Codigo,
                                       n.empresa,
                                       n.CGC,
                                       n.CPF,
                                       n.INSCRICAO,
                                       n.ENDERECO,
                                       n.numero,
                                       n.BAIRRO,
                                       n.CIDADE,
                                       n.ESTADO
                                   }).First();

                      codigo = dados.Codigo.ToString();
                      nome = dados.empresa ?? " ";
                      cnpjCli = dados.CGC ?? " ";
                      cpfCli = dados.CPF ?? " ";
                      ieCli = dados.INSCRICAO ?? " ";
                      enderecoCli = dados.ENDERECO.Trim() ?? " ";
                      numeroCli = dados.numero ?? " ";
                      bairroCli = dados.BAIRRO ?? " ";
                      cidadeCli = dados.CIDADE ?? " ";
                      estadoCli = dados.ESTADO ?? " ";
                      tipo="2";
                  }


                  if (item.Substring(0, 1) == "C")
                  {
                      Int32 codCli = Convert.ToInt32(item.Substring(1, item.Length - 1));

                      var dados = (from n in entidade.clientes
                                   where n.Codigo == codCli
                                   select new
                                   {
                                       n.Codigo,
                                       n.Nome,
                                       n.cnpj,
                                       n.cpf,
                                       n.inscricao,
                                       n.endereco,
                                       n.numero,
                                       n.bairro,
                                       n.cidade,
                                       n.estado
                                   }).First();

                      codigo = dados.Codigo.ToString() ?? " ";
                      nome = dados.Nome ?? " ";
                      cnpjCli = dados.cnpj ?? " ";
                      cpfCli = dados.cpf ?? " ";
                      ieCli = dados.inscricao ?? " ";
                      enderecoCli = dados.endereco.Trim() ?? " ";
                      numeroCli = dados.numero ?? " ";
                      bairroCli = dados.bairro ?? " ";
                      cidadeCli = dados.cidade ?? " ";
                      estadoCli = dados.estado ?? " ";
                      tipo="1";
                  }
                  if (string.IsNullOrEmpty(ieCli))
                      ieCli = "ISENTO";
                  if (string.IsNullOrWhiteSpace(cnpjCli))
                      cnpjCli = cpfCli;
                  if (!conteudo88_1040.ToString().Contains(cnpjCli))
                  {
                      conteudo88_1040.AppendLine("88" +
                      (contadorReg88 + 1).ToString().PadLeft(9, '0') +
                        "10" +
                        "40" +
                        cnpjCli.PadLeft(14, '0').Substring(0, 14) +
                        ieCli.PadRight(14, ' ').Substring(0, 14) +
                        estadoCli +
                        nome.PadRight(35, ' ').Substring(0, 35) +
                        string.Format("{0:yyyyMMdd}", dataInicial.Date) +
                        string.Format("{0:yyyyMMdd}", dataFinal.Date) +
                        tipo);
                      contadorReg88++;
                  }
              };
              conteudo.Append(conteudo88_1040);
        }

        public void Registro90()
        {
            int contadorRegistro = contadorReg10 + contadorReg11 + contadorReg50 + contadorReg54 + contadorReg60 + contadorReg61 + contadorReg74+ contadorReg75 + contadorReg88 + 1;

            conteudo.AppendLine("90" +
                cnpj.PadLeft(14, ' ').Substring(0, 14) +
                ie.PadRight(14, ' ').Substring(0, 14) +
                "50" + contadorReg50.ToString().PadLeft(8, '0') +
                "54" + contadorReg54.ToString().PadLeft(8, '0') +
                "60" + contadorReg60.ToString().PadLeft(8, '0') +
                "61" + contadorReg61.ToString().PadLeft(8, '0') +
                "74" + contadorReg74.ToString().PadLeft(8, '0') +
                "75" + contadorReg75.ToString().PadLeft(8, '0') +
                "88" + contadorReg88.ToString().PadLeft(8, '0') +
                "99" + contadorRegistro.ToString().PadLeft(8, '0') +
                " ".PadRight(21, ' ') + "1"
                );

            string nomeArquivoDestino = @"C:\iqsistemas\sice.net\exportacao\SEF\" + nomeArquivo; //  @ConfigurationManager.AppSettings["dirMovimentoECF"] + @"\Entradas.txt";
            using (FileStream fs = File.Create(@nomeArquivoDestino))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("ISO-8859-1")))
                {
                    sw.Write(@conteudo);
                }
            };
        }


        public static List<LayoutSintegra> EstruturaLayoutSEF()
        {
            var dados = new[] 
            {
              new LayoutSintegra {codigo=1,descricao="1 - Estrutura conforme Convênio ICMS 57/95 na versão do convêncio ICMS 31/99"},
              new LayoutSintegra {codigo=2,descricao="2 - Estrutura conforme Convênio ICMS 57/95 na versão atual"}
            };
            return dados.ToList();
        }

        public static List<FinalidadeSintegra> FinalidadeArquivoSEF()
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
