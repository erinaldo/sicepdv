using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.EntityClient;
using System.Data;

namespace SICEpdv
{
    /// <summary>
    /// Usando o ATO COTEPE 09/08 http://www.fazenda.gov.br/confaz/confaz/atos/atos_cotepe/2008/AC009_08.htm
    /// unidade de medidas oficiais: http://alice.desenvolvimento.gov.br/portalmdic/sitio/interna/interna.php?area=5&menu=1090&refr=605#Abreviaturas
    /// </summary>
    class SPEDFiscal : dadosFiscaisEmpresa
    {
        public List<string> registros { get; set; }
        List<string> unidades = new List<string>();
        private siceEntities entidade;
        public List<ModeloDocFiscal> modeloDocFiscal { get; set; }
        /* modeloDocFiscal
        * 01 = Nota Fiscal modelo 1 ou 1A - Nota Fiscal impressa
        * 1B = Nota Fiscal Avusal
        * 04 = Nota Fiscal de produto
        * 55 = Nota Fiscal Eletronica
        * 02 = Venda a Consumidor
        * 2D = Venda com ECF
        * Inventario = Exportação do Inventário
        */

        // Criar Entrada no Fomr
        public string codigoLayout { get; set; } //default 003 - 02-cod_ver Código da versão do leiaute conforme a tabela indicada no Ato COTEPE
        public string codifoFinalidade { get; set; } // Código Finalidade Remessa de Arquivo Original - Remessa de Arquivo Substituo
        public string codigoPerfil { get; set; }
        public int numeroInventario { get; set; }
        public int anoInventario { get; set; }

        // Opções de exportação de registro
        public bool gerarRegC100Entrada { get; set; }
        public bool gerarRegC100Saida { get; set; }
        public bool gerarRegC300 { get; set; }
        public bool gerarRegC350 { get; set; }
        public bool gerarRegC400 { get; set; }

        public bool gerarRegC500 { get; set; }

        List<string> codigos = new List<string>();
        List<string> codigoParticipante = new List<string>();

        // String do conteúdo dos arquivos: Na criação da varíavel do conteúdo 
        // seguir a sequência do layout SPED e adicionar na variável conteúdo
        // Para ficar seguindo o layout 
        public StringBuilder conteudo = new StringBuilder();
        StringBuilder conteudoReg0000 = new StringBuilder();
        StringBuilder conteudoReg0001 = new StringBuilder();
        StringBuilder conteudoReg0005 = new StringBuilder();
        StringBuilder conteudoReg0100 = new StringBuilder();
        StringBuilder conteudoReg0150 = new StringBuilder();
        StringBuilder conteudoReg0190 = new StringBuilder();
        StringBuilder conteudoReg0200 = new StringBuilder();
        StringBuilder conteudoReg0990 = new StringBuilder();

        StringBuilder conteudoRegC001 = new StringBuilder();
        StringBuilder conteudoRegC100 = new StringBuilder();
        StringBuilder conteudoRegC300 = new StringBuilder();
        StringBuilder conteudoRegC400 = new StringBuilder();
        StringBuilder conteudoRegC500 = new StringBuilder();


        StringBuilder conteudoRegC990 = new StringBuilder();

        StringBuilder conteudoRegD001 = new StringBuilder();

        StringBuilder conteudoRegE001 = new StringBuilder();

        StringBuilder conteudoRegG001 = new StringBuilder();

        StringBuilder conteudoRegH001 = new StringBuilder();

        StringBuilder conteudoReg1001 = new StringBuilder();

        StringBuilder conteudoReg9001 = new StringBuilder();

        // Apuração Bloco E 
        private decimal totDebitos = 0, ajDebitos = 0, totAjDebitos = 0, estornoCreditos = 0,
      totCreditos = 0, ajCreditos = 0, totAjCreditos = 0, estornoDebitos = 0, saldoCredorAnt = 0,
      saldoApurado = 0, totDeducoes = 0, icmsRecolher = 0, saldoCredorTran = 0, extraApuracao = 0;

        // Contadores de REgistros
        private int totalReg0 = 0;
        private int totalRegC = 0;
        private int totalRegD = 0;
        private int totalRegE = 0;
        private int totalRegG = 0;
        private int totalRegH = 0;
        private int totalReg1 = 0; // Bloco 1
        private int totalReg9 = 0;

        // Registro BLOCO 0
        private int contadorReg0001 = 0;
        private int contadorReg0005 = 0;
        private int contadorReg0150 = 0;
        private int contadorReg0100 = 0;
        private int contadorReg0190 = 0;
        private int contadorReg0200 = 0;

        // Registros BLOCO C
        private int contadorRegC100 = 0;
        private int contadorRegC170 = 0;
        private int contadorRegC190 = 0;
        private int contadorRegC300 = 0;
        private int contadorRegC320 = 0;
        private int contadorRegC321 = 0;
        private int contadorRegC350 = 0;
        private int contadorRegC370 = 0;
        private int contadorRegC390 = 0;
        private int contadorRegC400 = 0;
        private int contadorRegC405 = 0;
        private int contadorRegC420 = 0;
        private int contadorRegC460 = 0;
        private int contadorRegC470 = 0;
        private int contadorRegC490 = 0;

        private int contadorRegC500 = 0;
        private int contadorRegC510 = 0;
        private int contadorRegC590 = 0;


        // REgistro BLOCO H
        private int contadorRegH010 = 0;


        // Criar entradas
        //Construtor inicializando variáveis na criação do objeto
        public SPEDFiscal()
        {
            ObterDados();
            entidade = Conexao.CriarEntidade();
            registros = new List<string>();
            codigoLayout = "008";
            codifoFinalidade = "0";
            codigoPerfil = "A";
            numeroInventario = 0;
            // Iniciando registro;
            gerarRegC100Entrada = true;
            gerarRegC100Saida = true;
            gerarRegC300 = true;
            gerarRegC350 = true;
            gerarRegC400 = true;
            gerarRegC500 = true;
        }

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
            comp = dados.complemento;
            bairro = dados.bairro;
            fone = dados.telefone1;
            fax = dados.telefone2;
            email = dados.email;
            // Contador
            nomeContador = dados.contador;
            cpfContador = dados.cpfcontador ?? "";
            CRC = dados.crccontador;
            cnpjContador = dados.cnpjcontador;
            cepContador = dados.cepcontador;
            endContador = dados.enderecocontador.Trim();
            EndNumeroContador = dados.numerocontador;
            complementoContador = dados.complementocontador;
            bairroContador = dados.bairrocontador;
            foneContador = dados.telefonecontador;
            faxContador = dados.faxcontador;
            emailContador = dados.emailcontador;
            ind_ativ = dados.indicadoratividade.Substring(0, 1);

            // Variáveis 

        }

        public StringBuilder GerarSPEDFiscal(bool paf = false)
        {
            Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);
            #region BLOCO C
            BlocoC("abertura");
            BlocoCRegC100();
            //BlocoCRegC300(); // Perfil B
            BlocoCRegC350(); // Perfil A
            BlocoCRegC400(); // Emitidas por ECF modelo doc fiscal 2D            
            BlocoC("fechamento");
            #endregion BLOCO C

            BlocoD("abertura"); BlocoD("fechamento");
            BlocoE("abertura", paf == true ? "0" : "1");
            if (paf)
            {
                BlocoERegE100();
                BlocoERegE110();
                BlocoEReg116();
            }
            BlocoE("fechamento");
            BlocoG("abertura"); BlocoG("fechamento");
            BlocoH();
            Bloco1("abertura");
            Bloco1Reg1010();
            Bloco1("fechamento");
            // Bloco 0 -  nesta posição por que depois que processou as entradas e saída é que se obteu
            // os códigos e as unidades utilizadas

            Bloco0();
            Bloco9();

            MontarConteudo();

            return conteudo;
        }

        private void MontarConteudo()
        {
            // Pegando as informações dos códigos usados
            // Não alterar sequência. 
            conteudo.Append(conteudoReg0000);
            conteudo.Append(conteudoReg0001);
            conteudo.Append(conteudoReg0005);
            conteudo.Append(conteudoReg0100);
            conteudo.Append(conteudoReg0150);
            conteudo.Append(conteudoReg0190);
            conteudo.Append(conteudoReg0200);
            conteudo.Append(conteudoReg0990);

            conteudo.Append(conteudoRegC001);
            conteudo.Append(conteudoRegC100);
            conteudo.Append(conteudoRegC300);
            conteudo.Append(conteudoRegC400);
            conteudo.Append(conteudoRegC990);

            conteudo.Append(conteudoRegD001);

            conteudo.Append(conteudoRegE001);

            conteudo.Append(conteudoRegG001);

            conteudo.Append(conteudoRegH001);

            conteudo.Append(conteudoReg1001);

            conteudo.Append(conteudoReg9001);
        }

        private void BlocoC(string acao)
        {
            if (acao == "abertura")
            {
                string indicadorMovimento = "1";
                // Verificando se há movimento
                var dadosSaida = from n in Conexao.CriarEntidade().contnfsaida
                                 where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                 && (n.modelodocfiscal == "55" || n.modelodocfiscal == "01")
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select n;
                if (dadosSaida.Count() > 0)
                    indicadorMovimento = "0";

                var dadosEntradas = from n in Conexao.CriarEntidade().moventradas
                                    where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                                    && (n.modeloNF == "55" || n.modeloNF == "01")
                                    && n.Codigofilial == GlbVariaveis.glb_filial
                                    select n;
                if (dadosEntradas.Count() > 0)
                    indicadorMovimento = "0";

                var movimentoECF = (from n in Conexao.CriarEntidade().contdocs
                                    where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                    && n.modeloDOCFiscal == "2D"
                                    select new { n.data, n.modeloDOCFiscal });
                if (movimentoECF.Count() > 0)
                    indicadorMovimento = "0";


                if (!gerarRegC100Entrada && !gerarRegC100Saida && !gerarRegC400)
                    indicadorMovimento = "1";

                #region BLOCO C Registro C001: Abertura do Bloco C

                conteudoRegC001.AppendLine("|C001|" +
                   indicadorMovimento + "|");
                registros.Add("C001|1");
                totalRegC++;
                #endregion C001
            };
            if (acao == "fechamento")
            {
                #region BLOCO C Registro C990: Encerramento do Bloco C
                totalRegC += (contadorRegC100 + contadorRegC170 + contadorRegC190 + contadorRegC350 + contadorRegC370 + contadorRegC390 + contadorRegC400 +
                contadorRegC405 + contadorRegC420 + contadorRegC460 + contadorRegC470 + contadorRegC490 + contadorRegC500 + contadorRegC510 + contadorRegC590) + 1;

                conteudoRegC990.AppendLine("|C990|" +
                    totalRegC.ToString() + "|");
                registros.Add("C990|1");
                #endregion Registro C990
            }
        }

        private void BlocoCRegC400()
        {
            if (!gerarRegC400)
                return;

            #region BLOCO C Registro C400 -->C405-->C420: Equipamento ECF (Codigo 02 e 2D)

            #region BLOCO C Registro C400: Equipamento ECF (codigo 02 e 2D)
            //var dadosc400 = (from n in entidade.r02                            
            //                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
            //                && n.codigofilial == GlbVariaveis.glb_filial                            
            //                select n);

            //var dadosRegistroC400 = dadosc400.ToList();

            var ecfs = (from n in entidade.r02
                        where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                        && n.codigofilial == GlbVariaveis.glb_filial
                        select new { n.fabricacaoECF, n.codigofilial }).Distinct().ToList();
            //Registro PAI 

            siceEntities entidadeItens = Conexao.CriarEntidade();

            var obterDadosRegC470Geral = from n in entidadeItens.blococregc470
                                         where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                         && n.quantidade > 0
                                         select n;
            var dadosRegC470Geral = obterDadosRegC470Geral.ToList();


            var obterDadosRegC490Geral = from n in entidade.blococregc490
                                         where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                         select n;

            var dadosRegC490Geral = obterDadosRegC490Geral.ToList();


            foreach (var ecf in ecfs)
            {
                try
                {
                    var C400 = (from n in entidade.r02
                                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                && n.fabricacaoECF == ecf.fabricacaoECF
                                && n.codigofilial == GlbVariaveis.glb_filial
                                select new { n.data, n.modeloECF, n.fabricacaoECF, n.numeroECF }).Distinct().First();

                    conteudoRegC400.AppendLine("|C400|" +
                        "2D|" +
                        C400.modeloECF.Trim() + "|" +
                        C400.fabricacaoECF.Trim() + "|" +
                        C400.numeroECF + "|");
                    contadorRegC400++;
            #endregion C400;


                    #region BLOCO C Reigstro 405: Redução Z
                    var dadosC405 = from n in entidade.r02
                                    where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                    && n.fabricacaoECF == C400.fabricacaoECF
                                    && n.codigofilial == GlbVariaveis.glb_filial
                                    orderby n.data
                                    select new { n.data, n.cro, n.crz, n.coo, n.gtfinal, n.vendabrutadiaria };

                    var dadosRegistroC405 = dadosC405.ToList();

                    /*Registro Pai C405 
                     * O Registro Pai é selecionar por periodo e cada registro irá
                     * gerar a hierarquia dos registro filhos por da data da reducao
                     * Por isso os registro filho so tem o filtro por data que a data 
                     * corresponde no laco PAI.
                     */
                    foreach (var C405 in dadosRegistroC405)
                    {
                        conteudoRegC400.AppendLine("|C405|" +
                            string.Format("{0:ddMMyyyy}", C405.data.Value) + "|" +
                            C405.cro.Substring(3, 3) + "|" +
                            C405.crz + "|" +
                            C405.coo + "|" +
                            C405.gtfinal + "|" +
                            C405.vendabrutadiaria + "|");
                        contadorRegC405++;
                    #endregion Registro 405
                        #region Registro C420: Registro dos Totalizadores Parciais da Redução Z
                        var dadosRegC420 = from n in entidade.r03
                                           where n.data == C405.data.Value && n.fabricacaoECF == C400.fabricacaoECF
                                           && n.totalizadorParcial != "OPNF" && n.totalizadorParcial != "AT"
                                           && n.codigofilial == GlbVariaveis.glb_filial
                                           && n.valoracumulado > 0
                                           select new { n.totalizadorParcial, n.valoracumulado };

                        var dadosRegC490 = from n in dadosRegC490Geral // entidade.blococregc490
                                           where n.data == C405.data.Value && n.ecffabricacao == C400.fabricacaoECF
                                           orderby n.icms
                                           select n;

                        var regC490 = dadosRegC490.ToList();
                        var regC420 = dadosRegC420.ToList();

                        // Pegando os acumuladores de acordo com o RegC490 para que não haja diferença

                        foreach (var item in regC420)
                        {
                            decimal valorAcumulado = item.valoracumulado.Value;

                            string nrtotalizador = "";
                            if (item.totalizadorParcial.StartsWith("0"))
                                nrtotalizador = item.totalizadorParcial.Substring(0, 2);

                            conteudoRegC400.AppendLine("|C420|" +
                                item.totalizadorParcial.Trim() + "|" +
                                valorAcumulado + "|"
                                + nrtotalizador + "|" +
                                "|");
                            contadorRegC420++;
                        }

                        #endregion C420;
                        #region BLOCO C Registro C460: Documento Fiscal emitor por ECF (codigo 02 e 2D)
                        var dadosRegC460 = from n in entidade.contdocs
                                           where n.data == C405.data.Value && n.ecffabricacao == C400.fabricacaoECF
                                           && n.modeloDOCFiscal == "2D"
                                           && n.estornado == "N"
                                           && n.total > 0
                                           && n.CodigoFilial == GlbVariaveis.glb_filial
                                           select new
                                           {
                                               n.documento,
                                               n.estornado,
                                               n.Totalbruto,
                                               n.desconto,
                                               n.encargos,
                                               n.total,
                                               n.ncupomfiscal,
                                               n.ecfCPFCNPJconsumidor,
                                               n.ecfConsumidor
                                           };
                        List<string> cooAdd = new List<string>();

                        var regC460 = dadosRegC460.ToList();


                        foreach (var item in regC460)
                        {

                            var dadosRegC470 = from n in dadosRegC470Geral  // entidadeItens.blococregc470
                                               where n.documento == item.documento
                                               && n.quantidade > 0
                                               select n;
                            var regC470 = dadosRegC470.ToList();

                            if (regC470.Count > 0)
                            {

                                decimal? valorLiqDoc = item.Totalbruto - item.desconto + item.encargos;

                                decimal? somaValorDoc = (from n in dadosRegC470
                                                         select (decimal?)n.total).Sum();
                                if (somaValorDoc.HasValue && valorLiqDoc < somaValorDoc.Value)
                                    valorLiqDoc = somaValorDoc.Value;


                                var cod_sit = item.estornado == "N" ? "00" : "02";
                                string totalLiquido = valorLiqDoc.ToString().Replace(".", "");
                                string dataDoc = string.Format("{0:ddMMyyyy}", C405.data.Value);
                                string ecfCPFConsum = item.ecfCPFCNPJconsumidor.Trim();
                                string coo = item.ncupomfiscal;
                                if (ecfCPFConsum == "0" || ecfCPFConsum.Length < 11)
                                    ecfCPFConsum = "";

                                string ecfConsum = item.ecfConsumidor.Trim();
                                string valorPis = "0";
                                string valorCofins = "0";
                                if (item.estornado == "S")
                                {
                                    totalLiquido = "";
                                    dataDoc = "";
                                    ecfCPFConsum = "";
                                    ecfConsum = "";
                                    valorPis = "";
                                    valorCofins = "";
                                }
                                if (cooAdd.Contains(coo))
                                    coo = item.documento.ToString().PadLeft(6, '0').Substring(0, 6);

                                conteudoRegC400.AppendLine("|C460|" +
                                    "2D|" +
                                    cod_sit + "|" +
                                    coo + "|" +
                                    dataDoc + "|" +
                                    (totalLiquido) + "|" +
                                    valorPis + "|" +
                                    valorCofins + "|" +
                                    ecfCPFConsum + "|" +
                                    ecfConsum + "|");
                                contadorRegC460++;
                                cooAdd.Add(coo);



                                if (item.estornado == "N")
                                {
                                    #region BLOCO C Registro C470: Itens do Documento Fiscal emitor por ECF (codigo 02 e 2D)


                                    foreach (var itemRegC470 in regC470)
                                    {
                                        string cstICMS = "0" + itemRegC470.tributacao;
                                        if (cstICMS == "00")
                                            cstICMS = "000";
                                        if (cstICMS == "080")
                                            cstICMS = "090";

                                        if (!codigos.Contains(itemRegC470.codigo))
                                            codigos.Add(itemRegC470.codigo);

                                        conteudoRegC400.AppendLine("|C470|" +
                                            itemRegC470.codigo + "|" +
                                            string.Format("{0:N3}", itemRegC470.quantidade).Replace(".", "") + "|" +
                                            "0|" +
                                            itemRegC470.unidade + "|" +
                                             string.Format("{0:N2}", (itemRegC470.total)).Replace(".", "") + "|" +
                                            cstICMS + "|" +
                                            itemRegC470.cfop.Replace(".", "") + "|" +
                                            itemRegC470.icms + "|" +
                                            "0|" + // VL_PIS
                                            "0|"); // VL_COFINS     
                                        contadorRegC470++;
                                    }
                                    #endregion C470
                                }
                            }
                        }
                        #endregion C460
                        #region BLOCO C Registro C490: Registro analitico do movimento diário (codigo 02 e 2D)


                        foreach (var item in regC490)
                        {
                            totDebitos += item.totalICMS.GetValueOrDefault();
                            decimal total = item.total.Value;
                            decimal baseCalculoICMS = item.baseCalculoICMS.Value;
                            decimal totalICMS = item.totalICMS.Value;
                            string cstICMS = "0" + item.tributacao;

                            if (cstICMS == "00")
                                cstICMS = "000";

                            decimal? valorReg420 = item.total.Value;

                            if (item.icms > 0)
                            {
                                valorReg420 = (from n in regC420
                                               where n.totalizadorParcial.Contains(item.icms.ToString().PadLeft(2, '0'))
                                               select (decimal?)n.valoracumulado).FirstOrDefault();
                                if (valorReg420.HasValue && valorReg420 != total)
                                {
                                    total = valorReg420.Value;
                                }

                                // Passado a base de Cálculo do ICMS para não haver diferençca
                                // entre o valor da operação e base de cálculo pois quando o ICMS
                                // for maior que 0 então esses valores sempre serão iguais.
                                //if (total != baseCalculoICMS)
                                //{
                                //    baseCalculoICMS = total;
                                //    //descontoValor = (quantidade * precooriginal) * descontoPerc / 100;
                                //    //descontoValor = Math.Truncate(descontoValor * 100) / 100;
                                //    totalICMS = (total * item.icms / 100);
                                //    totalICMS = Math.Truncate(totalICMS * 100) / 100;
                                //}
                            }

                            conteudoRegC400.AppendLine("|C490|" +
                                cstICMS + "|" +
                                item.cfop.Replace(".", "") + "|" +
                                item.icms + "|" +
                                total.ToString().Replace(".", "") + "|" +
                                baseCalculoICMS.ToString().Replace(".", "") + "|" +
                                totalICMS.ToString().Replace(".", "") + "|" +
                                "|");
                            contadorRegC490++;
                        }

                        #endregion C490
                    }
                    //aqui
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Uma exceção :" + ex.Message);
                };
            }
            registros.Add("C400|" + contadorRegC400.ToString());
            registros.Add("C405|" + contadorRegC405.ToString());
            registros.Add("C420|" + contadorRegC420.ToString());
            registros.Add("C460|" + contadorRegC460.ToString());
            registros.Add("C470|" + contadorRegC470.ToString());
            registros.Add("C490|" + contadorRegC490.ToString());
            #endregion Registro C400
        }

        //Nota Fiscal Conta Energia Eletrica - 06 - A´gua canalizada 28 - Fornecimento de Gás - 29
        private void BlocoCRegC500()
        {
            if (!gerarRegC500)
                return;

            #region BLOCO C Registro C500 -> C510-->C590
            //Apena os modelo de documentos "01", "1B", "04", "55"
            var modeloDocumento = from n in modeloDocFiscal
                                  orderby n.tipo
                                  where (n.modeloDocFiscal.Contains("06") || n.modeloDocFiscal.Contains("29")
                                  || n.modeloDocFiscal.Contains("28"))
                                  select n;

            foreach (var item in modeloDocumento)
            {
                #region Entrada
                if (item.tipo == "Entrada")
                {
                    var dadosEntradas = from n in entidade.moventradas
                                        where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                                        && n.modeloNF == item.modeloDocFiscal
                                        && n.Codigofilial == GlbVariaveis.glb_filial
                                        select n;
                    var RegistroEntrada = dadosEntradas.ToList();

                    foreach (var itemNF in RegistroEntrada)
                    {

                        if (!codigoParticipante.Contains("F" + itemNF.codigofornecedor.ToString()))
                            codigoParticipante.Add("F" + itemNF.codigofornecedor.ToString());

                        string emitente = itemNF.Emitente == "P" ? "0" : "1";

                        //string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";
                        string situacaoNF = itemNF.situacaoNF;
                        if (situacaoNF == "S")
                            situacaoNF = "02";
                        if (situacaoNF == "N")
                            situacaoNF = "00";
                        if (situacaoNF.Trim().Count() == 1)
                            situacaoNF = "00";

                        conteudoRegC500.AppendLine("|C500|" +
                        "0|" + //IND_OPER 0:Entrada 1:Saida
                         emitente + "|" + // IND_EMIT
                        itemNF.codigofornecedor + "F|" +
                        itemNF.modeloNF + "|" +
                        situacaoNF + "|" +
                        itemNF.serie + "|" +
                        itemNF.subserie + "|" +
                        "02|" + // Consumo próprio 
                        itemNF.NF + "|" +
                        string.Format("{0:ddMMyyyy}", itemNF.DataEmissao) + "|" +
                        string.Format("{0:ddMMyyyy}", itemNF.dataEntrada) + "|" +
                        itemNF.ValorNota.ToString().Replace(".", "") + "|" +
                        itemNF.descontos.ToString().Replace(".", "") + "|" +
                        itemNF.ValorNota.ToString().Replace(".", "") + "|" +
                        0.ToString().Replace(".", "") + "|" +
                        0.ToString().Replace(".", "") + "|" + // VL_TERC
                        itemNF.Despesas.ToString().Replace(".", "") + "|" + // VL_DA Despesas Acessorias                       
                        itemNF.BaseIcms.ToString().Replace(".", "") + "|" +
                        itemNF.Icms.ToString().Replace(".", "") + "|" +
                        itemNF.BaseIcmsSubst.ToString().Replace(".", "") + "|" +
                        itemNF.IcmsSubst.ToString().Replace(".", "") + "|" +
                        "|" +
                        itemNF.pis.ToString().Replace(".", "") + "|" +
                        itemNF.cofins.ToString().Replace(".", "") + "|" +
                        "0|" +
                        "|"); // COD_GRUPO_TENSAO
                        contadorRegC500++;
                        // REtirado pois nao necessita apresentar quando for entrada conforme manual de orientacao

                        //var dadosItens = from n in entidade.registro50entradas_itens
                        //                 where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                        //                 && n.numero == itemNF.numero
                        //                 select n;
                        //foreach (var registroItem in dadosItens)
                        //{
                        //    if (!codigos.Contains(registroItem.codigo))
                        //        codigos.Add(registroItem.codigo);

                        //    conteudoRegC500.AppendLine("|C510|" + //REG
                        //        registroItem.sequencia.ToString() + "|" + //NUM_ITEM
                        //        registroItem.codigo + "|" + // COD_ITEM
                        //        registroItem.descricao + "|" + // DESCR_COMPL
                        //        string.Format("{0:N5}", registroItem.quantidade).Replace(".", "") + "|" + //QTD
                        //        registroItem.unidade + "|" + //? UNIDADE
                        //        string.Format("{0:N2}", registroItem.custo).Replace(".", "") + "|" + // VL_ITEM
                        //        "0" + "|" + // VL_DESC
                        //        "0" + "|" + // IND_MOV
                        //        registroItem.tributacao + "|" + //CST_ICMS
                        //        registroItem.cfopentrada.Replace(".", "") + "|" +
                        //        "|" + // COD_NAT  
                        //        registroItem.bcicms.Value.ToString().Replace(".", "") + "|" + // VL_BC_ICMS
                        //        registroItem.icmsentrada + "|" + // ALIQ_ICMS
                        //        registroItem.toticms.ToString().Replace(".", "") + "|" + // VL_ICMS
                        //        "0|" + // VL_BC_ICMS_ST
                        //        "0|" + // ALIQ_ST
                        //        "0|" + // VL_ICMS_ST
                        //        "0|" + // IND_APUR
                        //        "|" + // CST_IPI
                        //        "|" + // COD_ENQ
                        //        "0|" + // VL_BC_IPI
                        //        "0|" + // ALIQ_IPI
                        //        "0|" + // VL_IPI
                        //        "|" + // CST_PIS
                        //        "0|" + // VL_BC_PIS
                        //        "0|" + // ALIQ_PIS
                        //        "0|" + // QUANT_BC_PIS
                        //        "0|" + // ALIQ_PIS
                        //        "0|" + // VL_PIS
                        //        "|" + // CST_COFINS
                        //        "0|" + // VL_BC_COFINS
                        //        "0|" + // ALIQ_COFINS
                        //        "0|" + // QUANT_BC_COFINS
                        //        "0|" + // ALIQ_COFINS
                        //        "0|" + // VL_COFINS
                        //        "|"); // COD_CTA 
                        //    contadorRegC510++;
                        //}

                        var dadosItensAgr = from n in entidade.blococregc190
                                            where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                            && n.numero == itemNF.numero
                                            select n;
                        var dadosRegistrC190 = dadosItensAgr.ToList();

                        foreach (var regC190 in dadosRegistrC190)
                        {
                            conteudoRegC500.AppendLine("|C590|" +
                                regC190.tributacao.PadLeft(3, '0').Substring(0, 3) + "|" +
                                regC190.cfopentrada.Replace(".", "").Substring(0, 4) + "|" +
                                Funcoes.FormatarZerosEsquerda(regC190.icmsentrada, 6, false) + "|" +
                                regC190.totalNF.ToString().Replace(".", "") + "|" +
                                regC190.bcicms.ToString().Replace(".", "") + "|" +
                                regC190.toticms.ToString().Replace(".", "") + "|" +
                                Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                Funcoes.FormatarZerosEsquerda(regC190.totalReducaoICMS.Value, 12, true) + "|" +
                                Funcoes.FormatarZerosEsquerda(regC190.ipiItem.Value, 12, true) + "|" +
                                "|");
                            contadorRegC590++;
                        }

                    }
                };
                #endregion Entrada

            };

            if (contadorRegC500 > 0)
            {
                registros.Add("C500|" + contadorRegC500.ToString());
                //registros.Add("C510|" + contadorRegC510.ToString());
                registros.Add("C590|" + contadorRegC590.ToString());

            }

            #endregion Registro C100
        }


        private void BlocoCRegC100(bool gerarC170 = false)
        {
            if (!gerarRegC100Entrada && !gerarRegC100Saida)
                return;

            #region BLOCO C Registro C100 -> C170-->C190
            //Apena os modelo de documentos "01", "1B", "04", "55"
            var modeloDocumento = from n in modeloDocFiscal
                                  orderby n.tipo
                                  where (n.modeloDocFiscal.Contains("01") || n.modeloDocFiscal.Contains("1B")
                                  || n.modeloDocFiscal.Contains("04") || n.modeloDocFiscal.Contains("55"))
                                  select n;

            foreach (var item in modeloDocumento)
            {
                if (gerarRegC100Entrada)
                {
                    #region Entrada
                    if (item.tipo == "Entrada")
                    {
                        //var dadosEntradas = from n in entidade.moventradas
                        //                    where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                        //                    && n.modeloNF == item.modeloDocFiscal
                        //                    && n.Codigofilial == GlbVariaveis.glb_filial
                        //                    && n.exportarfiscal == "S"
                        //                    && n.lancada=="X"
                        //                    select n;

                        var dadosEntradas = from n in entidade.moventradas
                                            where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal.Date
                                            && n.modeloNF == item.modeloDocFiscal
                                            && n.Codigofilial == GlbVariaveis.glb_filial
                                            && n.exportarfiscal == "S"
                                                // Notas fiscais somente de Terceiro por notas próprias 
                                                // já está na tabela contnfsaida
                                            && n.Emitente == "T"
                                            && n.lancada == "X"
                                            && n.NF != ""
                                            select new
                                            {
                                                n.dataEntrada,
                                                n.numero,
                                                n.DataEmissao,
                                                n.indicadorpagamento,
                                                n.tipofrete,
                                                n.valorseguro,
                                                n.descontos,
                                                n.Frete,
                                                n.Despesas,
                                                n.pis_st,
                                                n.cofins_st,
                                                n.codigofornecedor,
                                                n.situacaoNF,
                                                n.Emitente,
                                                n.modeloNF,
                                                n.serie,
                                                n.NF,
                                                n.chave_nfe

                                            };
                        var RegistroEntrada = dadosEntradas.ToList();

                        var obterDadosGeral = from n in Conexao.CriarEntidade().registro50entradas_itens
                                              where n.dataentrada >= dataInicial.Date && n.dataentrada <= dataFinal.Date
                                              && n.codigofilial == GlbVariaveis.glb_filial
                                              select new
                                              {
                                                  n.totalNF,
                                                  n.totalProduto,
                                                  n.bcicms,
                                                  n.toticms,
                                                  n.baseCalculoIPI,
                                                  n.baseCalculoPIS,
                                                  n.totPIS,
                                                  n.totCOFINS,
                                                  n.baseCalculoCOFINS,
                                                  n.ipiItem,
                                                  n.unidade,
                                                  n.codigo,
                                                  n.tributacao,
                                                  n.descricao,
                                                  n.quantidade,
                                                  n.cfopentrada,
                                                  n.icmsentrada,
                                                  n.cstpis,
                                                  n.cstcofins,
                                                  n.pis,
                                                  n.cofins,
                                                  n.ipi,
                                                  n.dataentrada,
                                                  n.numero,
                                                  n.icmsst,
                                                  n.bcicmsST,
                                                  n.valoricmsST
                                              };
                        var dadosGeral = obterDadosGeral.ToList();


                        var obterdadosC190 = from n in entidade.blococregc190
                                             where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                             && n.totalProduto > 0
                                             select n;
                        var dadosItensAgrGeral = obterdadosC190.ToList();


                        foreach (var itemNF in RegistroEntrada)
                        {
                            try
                            {
                                var dadosItens = from n in dadosGeral // entidade.registro50entradas_itens
                                                 where n.dataentrada >= dataInicial.Date && n.dataentrada <= dataFinal.Date
                                                 && n.numero == itemNF.numero
                                                 && n.totalProduto > 0
                                                 select n;
                                var itensNota = dadosItens.ToList();

                                decimal totalNotaFiscal = (from n in itensNota select n.totalNF).Sum().Value;
                                decimal totalProdutos = (from n in itensNota select n.totalProduto).Sum().Value;

                                decimal baseCalculoICMS = (from n in itensNota select n.bcicms).Sum().Value;
                                decimal totalICMS = (from n in itensNota select n.toticms).Sum().Value;

                                decimal baseCalculoIPI = (from n in itensNota select n.baseCalculoIPI).Sum().Value;
                                decimal totalIPI = (from n in itensNota select n.ipiItem).Sum().Value;

                                decimal baseCalculoPIS = (from n in itensNota select n.baseCalculoPIS).Sum().Value;
                                decimal baseCalculoCOFINS = (from n in itensNota select n.baseCalculoCOFINS).Sum().Value;

                                decimal totalPIS = (from n in itensNota select n.totPIS).Sum().Value;
                                decimal totalCOFINS = (from n in itensNota select n.totCOFINS).Sum().Value;

                                decimal basecalculoICMSST = (from n in itensNota select n.bcicmsST).Sum().Value;
                                decimal totalICMSST = (from n in itensNota select n.valoricmsST).Sum().Value;

                                string dataEmissao = string.Format("{0:ddMMyyyy}", itemNF.DataEmissao);
                                string dataEntrada = string.Format("{0:ddMMyyyy}", itemNF.dataEntrada);
                                string indicadorPagamento = itemNF.indicadorpagamento;
                                decimal descontos = itemNF.descontos;
                                string tipoFrete = itemNF.tipofrete;
                                decimal valorFrete = itemNF.Frete;
                                decimal valorSeguro = itemNF.valorseguro;
                                decimal despesas = itemNF.Despesas;
                                decimal pisST = itemNF.pis_st;
                                decimal cofinsST = itemNF.cofins_st;

                                var chavenfe = itemNF.chave_nfe.Trim();
                                if (chavenfe.Length < 44)
                                    chavenfe = "";

                                // REgras para evitar erro de validacao. O usuário pode ter digitado a NF errado
                                // Aqui evita erro na validacao
                                if (baseCalculoICMS > totalProdutos)
                                    totalProdutos = baseCalculoICMS;

                                if (!codigoParticipante.Contains("F" + itemNF.codigofornecedor.ToString()))
                                    codigoParticipante.Add("F" + itemNF.codigofornecedor.ToString());

                                string emitente = itemNF.Emitente == "P" ? "0" : "1";

                                //string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";
                                string situacaoNF = itemNF.situacaoNF;
                                if (situacaoNF == "S")
                                    situacaoNF = "02";
                                if (situacaoNF == "N")
                                    situacaoNF = "00";

                                if (situacaoNF.Trim().Count() == 1)
                                    situacaoNF = "00";

                                conteudoRegC100.AppendLine("|C100|" +
                                "0|" + //IND_OPER 0:Entrada 1:Saida
                                 emitente + "|" + // IND_EMIT
                                itemNF.codigofornecedor + "F|" +
                                itemNF.modeloNF + "|" +
                                situacaoNF + "|" +
                                itemNF.serie + "|" +
                                itemNF.NF + "|" +
                                chavenfe + "|" +
                                dataEmissao + "|" +
                                dataEntrada + "|" +
                                totalNotaFiscal.ToString().Replace(".", "") + "|" +
                                indicadorPagamento + "|" +
                                descontos.ToString().Replace(".", "") + "|" +
                                0.ToString().Replace(".", "") + "|" +
                                totalProdutos.ToString().Replace(".", "") + "|" +
                                tipoFrete + "|" +
                                valorFrete.ToString().Replace(".", "") + "|" +
                                valorSeguro.ToString().Replace(".", "") + "|" +
                                despesas.ToString().Replace(".", "") + "|" +
                                baseCalculoICMS.ToString().Replace(".", "") + "|" +
                                totalICMS.ToString().Replace(".", "") + "|" +
                                basecalculoICMSST.ToString().Replace(".", "") + "|" + // Não está calculado o ST ainda itemNF.BaseIcmsSubst.ToString().Replace(".", "") + "|" +
                                totalICMSST.ToString().Replace(".", "") + "|" + // Não está calculado o ST ainda itemNF.IcmsSubst.ToString().Replace(".", "") + "|" +
                                totalIPI.ToString().Replace(".", "") + "|" +
                                totalPIS.ToString().Replace(".", "") + "|" +
                                totalCOFINS.ToString().Replace(".", "") + "|" +
                                pisST.ToString().Replace(".", "") + "|" +
                                cofinsST.ToString().Replace(".", "") + "|"); // VL_COFINS_ST
                                contadorRegC100++;


                                int sequencia = 1;
                                foreach (var registroItem in itensNota)
                                {
                                    if (!codigos.Contains(registroItem.codigo))
                                        codigos.Add(registroItem.codigo);

                                    conteudoRegC100.AppendLine("|C170|" + //REG
                                       sequencia.ToString() + "|" + //NUM_ITEM
                                        registroItem.codigo + "|" + // COD_ITEM
                                        registroItem.descricao.Trim().Replace("|", "") + "|" + // DESCR_COMPL
                                        string.Format("{0:N5}", registroItem.quantidade).Replace(".", "") + "|" + //QTD
                                        registroItem.unidade + "|" + //? UNIDADE
                                        string.Format("{0:N2}", registroItem.totalProduto).Replace(".", "") + "|" + // VL_ITEM
                                        "0" + "|" + // VL_DESC
                                        "0" + "|" + // IND_MOV
                                        registroItem.tributacao + "|" + //CST_ICMS
                                        registroItem.cfopentrada.Replace(".", "") + "|" +
                                        "|" + // COD_NAT  
                                        registroItem.bcicms.Value.ToString().Replace(".", "") + "|" + // VL_BC_ICMS
                                        registroItem.icmsentrada + "|" + // ALIQ_ICMS
                                        registroItem.toticms.ToString().Replace(".", "") + "|" + // VL_ICMS
                                        registroItem.bcicmsST.ToString().Replace(".", "") + "|" + // VL_BC_ICMS_ST
                                        registroItem.icmsst.ToString().Replace(".", "") + "|" + // ALIQ_ST
                                        registroItem.valoricmsST.ToString().Replace(".", "") + "|" + // VL_ICMS_ST
                                        "0|" + // IND_APUR
                                        "|" + // CST_IPI
                                        "|" + // COD_ENQ
                                        registroItem.baseCalculoIPI.Value.ToString().Replace(".", "") + "|" + // VL_BC_IPI
                                        registroItem.ipi.ToString().Replace(".", "") + "|" + // ALIQ_IPI
                                        registroItem.ipiItem.ToString().Replace(".", "") + "|" + // VL_IPI
                                        registroItem.cstpis + "|" + // CST_PIS
                                        registroItem.baseCalculoPIS.Value.ToString().Replace(".", "") + "|" + // VL_BC_PIS
                                        registroItem.pis.ToString().Replace(".", "") + "|" +  // ALIQ_PIS
                                        "|" + // QUANT_BC_PIS
                                        "|" + // ALIQ_PIS
                                        registroItem.totPIS.ToString().Replace(".", "") + "|" + // VL_PIS
                                        registroItem.cstcofins + "|" + // CST_COFINS
                                        registroItem.baseCalculoCOFINS.Value.ToString().Replace(".", "") + "|" + // VL_BC_COFINS
                                        registroItem.cofins.ToString().Replace(".", "") + "|" + // ALIQ_COFINS
                                        "|" + // QUANT_BC_COFINS
                                        "|" + // ALIQ_COFINS
                                        registroItem.totCOFINS.ToString().Replace(".", "") + "|" + // VL_COFINS
                                        "|"); // COD_CTA 
                                    contadorRegC170++;
                                    sequencia++;
                                }


                                var dadosItensAgr = from n in dadosItensAgrGeral // entidade.blococregc190
                                                    where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                                    && n.numero == itemNF.numero
                                                    && n.totalProduto > 0
                                                    select n;
                                var dadosRegistrC190 = dadosItensAgr.ToList();

                                foreach (var regC190 in dadosRegistrC190)
                                {
                                    // Excluído esse Cfop conforme documento
                                    if (regC190.cfopentrada != "1.605")
                                    {
                                        totCreditos += regC190.toticms.Value;
                                    }

                                    conteudoRegC100.AppendLine("|C190|" +
                                        regC190.tributacao.PadLeft(3, '0').Substring(0, 3) + "|" +
                                        regC190.cfopentrada.Replace(".", "").Substring(0, 4) + "|" +
                                        Funcoes.FormatarZerosEsquerda(regC190.icmsentrada, 6, false) + "|" +
                                        regC190.totalNF.ToString().Replace(".", "") + "|" +
                                        regC190.bcicms.ToString().Replace(".", "") + "|" +
                                        regC190.toticms.ToString().Replace(".", "") + "|" +
                                        regC190.bcicmsST.ToString().Replace(".", "") + "|" +
                                        regC190.valoricmsST.ToString().Replace(".", "") + "|" +
                                        Funcoes.FormatarZerosEsquerda(regC190.totalReducaoICMS.Value, 12, true) + "|" +
                                       regC190.ipiItem.ToString().Replace(".", "") + "|" +
                                        "|");
                                    contadorRegC190++;
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Nota de entrada com erro. Entrada número:" + itemNF.numero.ToString() + " Ex:" + ex.Message);
                            }
                        }

                    };
                    #endregion Entrada
                }
                #region Saida
                if (item.tipo == "Saida")
                {
                    ///  CFOPs que não são gerados 
                    ///  1.202 - DEV. VENDAS DE MERCADORIAS
                    ///  5.929 - LANÇAMENTO P/ SUBST. DE CUPON FISCAL-ECF
                    ///  
                    if (gerarRegC100Saida)
                    {
                        var dadosSaida = from n in entidade.contnfsaida
                                         where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                         && n.modelodocfiscal == item.modeloDocFiscal
                                         && n.codigofilial == GlbVariaveis.glb_filial
                                         && n.finalidade == "1"
                                         && n.codcliente > 0
                                         && n.exportarfiscal == "S"
                                         && n.chave_nfe != null
                                         && n.chave_nfe != ""
                                         && n.cfop != "5.929"
                                         && n.cfop != "1.202"
                                         select n;
                        var RegistroSaida = dadosSaida.ToList();


                        var ObterdadosItensGer = from n in entidade.registro50saidas_itens
                                                 where n.DATA >= dataInicial.Date && n.DATA <= dataFinal.Date
                                                 && n.codigofilial == GlbVariaveis.glb_filial
                                                 select n;
                        var dadosItensGer = ObterdadosItensGer.ToList();
                        bool processarNFe = true;
                        foreach (var itemNF in RegistroSaida)
                        {
                            processarNFe = true;
                            try
                            {
                                string notaFiscal = Convert.ToString(itemNF.notafiscal.Value).Trim();

                                var dadosItens = from n in dadosItensGer // entidade.registro50saidas_itens
                                                 where n.DATA >= dataInicial.Date && n.DATA <= dataFinal.Date
                                                 && n.notafiscal == notaFiscal && n.serienf == itemNF.serie
                                                 && n.codigofilial == GlbVariaveis.glb_filial
                                                 select n;
                                var itensNota = dadosItens.ToList();

                                string situacaoNF = itemNF.situacaoNF;
                                bool nfCancelada = false;
                                if (situacaoNF == "S" || situacaoNF == "02" || situacaoNF == "03" || situacaoNF == "04" && situacaoNF == "05")
                                    nfCancelada = true;

                                string tipoParticipante = "C";
                                if (itemNF.codcliente > 0 && !nfCancelada)
                                {

                                    var dadosCli = (from n in entidade.clientes
                                                    where n.Codigo == itemNF.codcliente
                                                    select new { n.cnpj, n.cpf, n.inscricao, n.estado }).FirstOrDefault();

                                    if (dadosCli == null)
                                    {
                                        processarNFe = false;
                                    }
                                    if (processarNFe)
                                    {
                                        if (!codigoParticipante.Contains("C" + itemNF.codcliente.ToString()))
                                            codigoParticipante.Add("C" + itemNF.codcliente.ToString());
                                    }
                                }
                                if (itemNF.codfornecedor > 0 && !nfCancelada)
                                {

                                    var dadosFor = (from n in entidade.fornecedores
                                                    where n.Codigo == itemNF.codfornecedor
                                                    select new { n.CGC, n.INSCRICAO, n.ESTADO, n.CPF }).FirstOrDefault();
                                    if (dadosFor == null)
                                        processarNFe = false;

                                    if (processarNFe)
                                    {
                                        if (!codigoParticipante.Contains("F" + itemNF.codfornecedor.ToString()))
                                            codigoParticipante.Add("F" + itemNF.codfornecedor.ToString());
                                        tipoParticipante = "F";
                                    }
                                }

                                if (itensNota.Count > 0 && processarNFe)
                                {

                                    decimal totalNotaFiscal = (from n in itensNota select n.SUM_TOTAL_).Sum().Value;
                                    decimal totalProdutos = (from n in itensNota select n.SUM_TOTAL_).Sum().Value;
                                    decimal baseCalculoICMS = (from n in itensNota select n.baseCalculoICMS).Sum().Value;
                                    decimal totalICMS = (from n in itensNota select n.totalicms).Sum().Value;

                                    decimal baseCalculoIPI = (from n in itensNota select n.baseCalculoIPI).Sum().Value;
                                    decimal totalIPI = (from n in itensNota select n.totalIPI).Sum().Value;

                                    decimal baseCalculoPIS = (from n in itensNota select n.baseCalculoPIS).Sum().Value;
                                    decimal baseCalculoCOFINS = (from n in itensNota select n.baseCalculoCOFINS).Sum().Value;

                                    decimal totalPIS = (from n in itensNota select n.totalPIS).Sum().Value;
                                    decimal totalCOFINS = (from n in itensNota select n.totalCOFINS).Sum().Value;
                                    //decimal baseCalculoICMS = (from n in ItensNota select n.baseCalculoICMS).Sum().Value;
                                    decimal basecalculoICMSST = 0;
                                    decimal totalICMSST = 0;


                                    string dataSaida = string.Format("{0:ddMMyyyy}", itemNF.data);
                                    string dataEmissao = string.Format("{0:ddMMyyyy}", itemNF.dataemissao);
                                    string indicadorPagamento = itemNF.indicadorpagamento;
                                    decimal descontos = itemNF.desconto;
                                    string tipoFrete = itemNF.tipofrete;
                                    decimal valorFrete = itemNF.totalfrete;
                                    decimal valorSeguro = itemNF.totalseguro;
                                    decimal despesas = itemNF.despesasacessorias;
                                    decimal pisST = 0;
                                    decimal cofinsST = 0;


                                    string emitente = "0";
                                    Int32 codDestinatario = itemNF.codcliente > 0 ? itemNF.codcliente : itemNF.codfornecedor;
                                    //string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";

                                    if (situacaoNF == "S")
                                        situacaoNF = "02";
                                    if (situacaoNF == "N")
                                        situacaoNF = "00";
                                    if (situacaoNF.Trim().Count() == 1)
                                        situacaoNF = "00";


                                    if (!nfCancelada)
                                    {
                                        conteudoRegC100.AppendLine("|C100|" +
                                        itemNF.tipo + "|" + //IND_OPER 0:Entrada 1:Saida
                                        emitente + "|" + // IND_EMIT
                                        codDestinatario.ToString() + tipoParticipante + "|" +
                                        itemNF.modelodocfiscal + "|" +
                                        situacaoNF + "|" +
                                        itemNF.serie + "|" +
                                        itemNF.notafiscal + "|" +
                                        itemNF.chave_nfe + "|" +
                                        dataSaida + "|" +
                                        dataEmissao + "|" +
                                        totalNotaFiscal.ToString().Replace(".", "") + "|" +
                                        indicadorPagamento + "|" +
                                        descontos.ToString().Replace(".", "") + "|" +
                                        0.ToString().Replace(".", "") + "|" +
                                        totalProdutos.ToString().Replace(".", "") + "|" +
                                        tipoFrete + "|" +
                                        valorFrete.ToString().Replace(".", "") + "|" +
                                        valorSeguro.ToString().Replace(".", "") + "|" +
                                        despesas.ToString().Replace(".", "") + "|" +
                                        baseCalculoICMS.ToString().Replace(".", "") + "|" +
                                        totalICMS.ToString().Replace(".", "") + "|" +
                                        basecalculoICMSST.ToString().Replace(".", "") + "|" +
                                        totalICMSST.ToString().Replace(".", "") + "|" +
                                        totalIPI.ToString().Replace(".", "") + "|" +
                                        totalPIS.ToString().Replace(".", "") + "|" +
                                        totalCOFINS.ToString().Replace(".", "") + "|" + // VL_COFINS
                                        pisST.ToString().Replace(".", "") + "|" +
                                        cofinsST.ToString().Replace(".", "") + "|"); // VL_COFINS_ST
                                    }
                                    //Nfs canceladas 
                                    if (nfCancelada)
                                    {
                                        conteudoRegC100.AppendLine("|C100|" +
                                          "1|" + //IND_OPER 0:Entrada 1:Saida
                                           emitente + "|" + // IND_EMIT
                                          "|" + // codDestinatario.ToString() + tipoParticipante 
                                          itemNF.modelodocfiscal + "|" +
                                          situacaoNF + "|" +
                                          itemNF.serie + "|" +
                                          itemNF.notafiscal + "|" +
                                          itemNF.chave_nfe + "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" +
                                         "|" + // VL_COFINS
                                         "|" +
                                         "|"); // VL_COFINS_ST
                                    }
                                    contadorRegC100++;


                                    if (gerarC170 == true && !nfCancelada)
                                    {

                                        int sequencia = 1;
                                        foreach (var registroItem in itensNota)
                                        {
                                            if (!codigos.Contains(registroItem.codigo))
                                                codigos.Add(registroItem.codigo);

                                            conteudoRegC100.AppendLine("|C170|" + //REG
                                                sequencia.ToString() + "|" + //NUM_ITEM
                                                registroItem.codigo + "|" + // COD_ITEM
                                                registroItem.produto.Trim().Replace("|", "") + "|" + // DESCR_COMPL
                                                string.Format("{0:N5}", registroItem.SUM_quantidade_).Replace(".", "") + "|" + //QTD
                                                registroItem.unidade + "|" + //? UNIDADE
                                                string.Format("{0:N2}", registroItem.SUM_TOTAL_).Replace(".", "") + "|" + // VL_ITEM
                                                "0" + "|" + // VL_DESC
                                                "0" + "|" + // IND_MOV
                                                "1" + registroItem.tributacao + "|" + //CST_ICMS
                                                registroItem.cfop.Replace(".", "") + "|" +
                                                "|" + // COD_NAT  
                                                registroItem.baseCalculoICMS.Value.ToString().Replace(".", "") + "|" + // VL_BC_ICMS
                                                registroItem.icms + "|" + // ALIQ_ICMS
                                                registroItem.totalicms.ToString().Replace(".", "") + "|" + // VL_ICMS                                        
                                                "0|" + // VL_BC_ICMS_ST
                                                "0|" + // ALIQ_ST
                                                "0|" + // VL_ICMS_ST
                                                "0|" + // IND_APUR
                                                "|" + // CST_IPI
                                                "|" + // COD_ENQ
                                                registroItem.baseCalculoIPI.Value.ToString().Replace(".", "") + "|" + // VL_BC_IPI
                                                registroItem.ipi.ToString().Replace(".", "") + "|" + // ALIQ_IPI
                                                registroItem.totalIPI.ToString().Replace(".", "") + "|" + // VL_IPI
                                                registroItem.cstpis + "|" + // CST_PIS
                                                registroItem.baseCalculoPIS.Value.ToString().Replace(".", "") + "|" + // VL_BC_PIS
                                                registroItem.pis.ToString().Replace(".", "") + "|" +  // ALIQ_PIS
                                                "|" + // QUANT_BC_PIS
                                                "|" + // ALIQ_PIS
                                                registroItem.totalPIS.ToString().Replace(".", "") + "|" + // VL_PIS
                                                registroItem.cstcofins + "|" + // CST_COFINS
                                                registroItem.baseCalculoCOFINS.Value.ToString().Replace(".", "") + "|" + // VL_BC_COFINS
                                                registroItem.cofins.ToString().Replace(".", "") + "|" + // ALIQ_COFINS
                                                "|" + // QUANT_BC_COFINS
                                                "|" + // ALIQ_COFINS
                                                registroItem.totalCOFINS.ToString().Replace(".", "") + "|" + // VL_COFINS
                                                "|"); // COD_CTA                         
                                            contadorRegC170++;
                                            sequencia++;
                                        }

                                    }

                                    if (!nfCancelada)
                                    {

                                        var dadosItensAgr = from n in Conexao.CriarEntidade().blococregc190_saida
                                                            where n.notafiscal == notaFiscal.Trim() && n.serienf == itemNF.serie
                                                            && n.codigofilial == GlbVariaveis.glb_filial
                                                            && n.DATA >= dataInicial.Date && n.DATA <= dataFinal.Date
                                                            select n;

                                        var dadosRegistroC190 = dadosItensAgr.ToList();

                                        foreach (var regC190 in dadosRegistroC190)
                                        {

                                            totDebitos += regC190.totalicms.GetValueOrDefault();

                                            conteudoRegC100.AppendLine("|C190|" +
                                                regC190.tributacao.PadLeft(3, '0').Substring(0, 3) + "|" +
                                                regC190.cfop.Replace(".", "").Substring(0, 4) + "|" +
                                                Funcoes.FormatarZerosEsquerda(regC190.icms, 6, false) + "|" +
                                                regC190.totalItem.ToString().Replace(".", "") + "|" +
                                                regC190.baseCalculoICMS.ToString().Replace(".", "") + "|" +
                                                regC190.totalicms.ToString().Replace(".", "") + "|" +
                                                Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                                Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                                Funcoes.FormatarZerosEsquerda(regC190.totalReducaoICMS.Value, 12, true) + "|" +
                                                totalIPI.ToString().Replace(".", "") + "|" +
                                                "|");
                                            contadorRegC190++;
                                        }
                                    }
                                } // if itens.count>0
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Nota de saída com erro. Entrada número:" + itemNF.numero.ToString() + " Ex:" + ex.Message);
                            }
                        };
                    };
                #endregion Saida
                } // If GerarSaida
            };

            if (contadorRegC100 > 0)
            {
                registros.Add("C100|" + contadorRegC100.ToString());
                registros.Add("C170|" + contadorRegC170.ToString());
                registros.Add("C190|" + contadorRegC190.ToString());

            }

            #endregion Registro C100
        }

        private void BlocoCRegC300()
        {
            if (!gerarRegC300)
                return;

            #region BLOCO C Registro C300->C31->C320->C321: RESUMO DIÁRIO DAS NOTAS FISCAIS DE VENDA A CONSUMIDOR (CÓDIGO 02)
            #region BLOCO C REgistro C300 -
            siceEntities entidade = Conexao.CriarEntidade();
            siceEntities entidadeC320 = Conexao.CriarEntidade();
            siceEntities entidadeC321 = Conexao.CriarEntidade();

            var regC300 = from n in entidade.blococregc300
                          where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                         && (n.modeloDOCFiscal == "02" || n.modeloDOCFiscal == "D1")
                         && n.ecfnumero == "" && n.estornado == "N"
                          select n;

            long numeroInicial = 0;
            long numeroFinal = 0;

            if (regC300.Count() > 0)
            {
                numeroFinal = (from n in regC300
                               select n.nrnotafiscal).Max();

                numeroInicial = (from n in regC300
                                 select n.nrnotafiscal).Min();
            };

            // Registro Pai
            foreach (var item in regC300)
            {
                conteudoRegC300.AppendLine("|C300|" +
                    item.modeloDOCFiscal + "|" +
                    item.serienf + "|" +
                    item.subserienf + "|" +
                    numeroInicial.ToString() + "|" +
                    numeroFinal.ToString() + "|" +
                    string.Format("{0:ddMMyyyy}", item.data) + "|" +
                    string.Format("{0:N2}", item.total).Replace(".", "") + "|" +
                    "0|" +
                    "0|" +
                    "|");
                contadorRegC300++;

                var dadosC320 = from n in entidadeC320.blococregc320
                                where n.data == item.data.Value
                                && n.serieNF == item.serienf && n.subserienf == item.subserienf
                                && n.modelodocfiscal == item.modeloDOCFiscal
                                select n;

                foreach (var regC320 in dadosC320)
                {
                    totDebitos += regC320.totalICMS.GetValueOrDefault();

                    conteudoRegC300.AppendLine("|C320|" +
                        regC320.tributacao + "|" +
                        regC320.cfop + "|" +
                        string.Format("{0:N2}", regC320.icms).Replace(".", "") + "|" +
                        string.Format("{0:N2}", regC320.total).Replace(".", "") + "|" +
                        string.Format("{0:N2}", regC320.bcICMS).Replace(".", "") + "|" +
                        string.Format("{0:N2}", regC320.totalICMS).Replace(".", "") + "|" +
                        "0|" +
                        "|");
                    contadorRegC320++;
                }

                var dadosC321 = from n in entidadeC321.blococregc321
                                where n.data == item.data.Value && n.modelodocfiscal == item.modeloDOCFiscal
                                && n.serieNF == item.serienf && n.subserienf == item.subserienf
                                select n;
                foreach (var regC321 in dadosC321)
                {
                    conteudoRegC300.AppendLine("|C321|" +
                        regC321.codigo + "|" +
                         string.Format("{0:N3}", regC321.quantidade).Replace(".", "") + "|" +
                         regC321.unidade + "|" +
                         string.Format("{0:N2}", regC321.total).Replace(".", "") + "|" +
                         string.Format("{0:N2}", regC321.descontovalor).Replace(".", "") + "|" +
                         string.Format("{0:N2}", regC321.bcICMS).Replace(".", "") + "|" +
                         string.Format("{0:N2}", regC321.totalICMS).Replace(".", "") + "|" +
                         string.Format("{0:N2}", regC321.totalPIS).Replace(".", "") + "|" +
                         string.Format("{0:N2}", regC321.totalCOFINS).Replace(".", "") + "|");
                    contadorRegC321++;
                }
                registros.Add("C300|" + contadorRegC300.ToString());
                registros.Add("C320|" + contadorRegC320.ToString());
                registros.Add("C321|" + contadorRegC321.ToString());
            }


            #endregion

            #endregion
        }

        private void BlocoCRegC350()
        {
            if (!gerarRegC350)
                return;

            #region BLOCO C Registro C350->C370->C390: Nota Fiscal de Venda a Consumidor (codigo 02)

            #region BLOCO C Registro C350: Nota Fiscal de Venda a Consumidor (codigo 02)
            var dados = (from n in entidade.contdocs
                         where (n.modeloDOCFiscal == "02" || n.modeloDOCFiscal == "D1")
                         && n.data >= dataInicial.Date && n.data <= dataFinal.Date
                         && n.CodigoFilial == GlbVariaveis.glb_filial
                         && n.estornado != "S"
                         select new
                         {
                             REG = "C350",
                             SER = "1",
                             SUB_SER = "",
                             NUM_DOC = n.documento,
                             DT_DOC = n.data,
                             CNPJ_CPF = n.ecfCPFCNPJconsumidor,
                             VL_MERC = n.Totalbruto,
                             VL_DOC = n.total,
                             VL_DESC = n.desconto,
                             VL_PIS = 0,
                             VL_COFINS = 0,
                             COD_CTA = ""
                         });
            var dadosRegistroC350 = dados.ToList();

            // Registro Pai 
            foreach (var item in dadosRegistroC350)
            {
                var dadosItens = from n in entidade.vendatmp
                                 where n.documento == item.NUM_DOC
                                 select new { n.nrcontrole, n.codigo, n.quantidade, n.unidade, n.total, n.descontovalor };

                var dadosItensVenda = from n in entidade.venda
                                      where n.documento == item.NUM_DOC
                                      select new { n.nrcontrole, n.codigo, n.quantidade, n.unidade, n.total, n.descontovalor };

                if (dadosItensVenda.Count() > 0)
                {

                    if (dadosItens.Count() == 0 || dadosItens == null)
                        dadosItens = dadosItensVenda;
                    else
                        dadosItens.Concat(dadosItensVenda);

                }

                if (dadosItens.Count() > 0)
                {
                    conteudoRegC300.AppendLine("|C350|" +
                        item.SER + "|" +
                        item.SUB_SER + "|" +
                        item.NUM_DOC + "|" +
                        string.Format("{0:ddMMyyyy}", item.DT_DOC) + "|" +
                        item.CNPJ_CPF + "|" +
                        string.Format("{0:N2}", item.VL_MERC).Replace(".", "") + "|" +
                        string.Format("{0:N2}", item.VL_DOC).Replace(".", "") + "|" +
                        string.Format("{0:N2}", item.VL_DESC).Replace(".", "") + "|" +
                        item.VL_PIS + "|" +
                        item.VL_COFINS + "|" +
                        item.COD_CTA + "|");
                    contadorRegC350++;
            #endregion C350
                    #region BLOCO C Registro C370: Itens do Documento



                    foreach (var itens in dadosItens)
                    {
                        if (!codigos.Contains(itens.codigo))
                            codigos.Add(itens.codigo);

                        conteudoRegC300.AppendLine("|C370|" +
                            itens.nrcontrole + "|" +
                            itens.codigo + "|" +
                            string.Format("{0:N3}", itens.quantidade).Replace(".", "") + "|" +
                            itens.unidade + "|" +
                            string.Format("{0:N2}", itens.total).Replace(".", "") + "|" +
                            itens.descontovalor + "|");
                        contadorRegC370++;
                    }
                    #endregion C370
                    #region BLOCO C Registro C390: Registro analitico das notas ficas de venda a consumidor (codigo 02)


                    var dadosAgrup = from n in entidade.blococregc390
                                     where n.documento == item.NUM_DOC
                                     select n;
                    foreach (var itensAgrup in dadosAgrup)
                    {
                        totDebitos += itensAgrup.totalICMS.GetValueOrDefault();

                        conteudoRegC300.AppendLine("|C390|" +
                            "0" + itensAgrup.tributacao + "|" +
                            itensAgrup.cfop.Replace(".", "") + "|" +
                            itensAgrup.icms + "|" +
                            itensAgrup.total + "|" +
                            string.Format("{0:N2}", itensAgrup.baseCalculoICMS).Replace(".", "") + "|" +
                            itensAgrup.totalICMS + "|" +
                            "0|" + // 08 VL_RED_BC
                            "|");
                        contadorRegC390++;
                    }

                } // if dados itens
                    #endregion C390
            }
            registros.Add("C350|" + contadorRegC350.ToString());
            registros.Add("C370|" + contadorRegC370.ToString());
            registros.Add("C390|" + contadorRegC390.ToString());

            #endregion Registro C350
        }

        private void Bloco9()
        {
            #region BLOCO 9
            int contadorReg9900 = 0;
            conteudoReg9001.AppendLine("|9001|" +
                "0|");
            registros.Add("9001|1");
            totalReg9++;

            foreach (var item in registros)
            {
                conteudoReg9001.AppendLine("|9900|" +
                    item + "|");
                contadorReg9900++;
            }
            contadorReg9900++; // Aqui por que ele esta se adicionando e adicionando o registro de fechamento;
            conteudoReg9001.AppendLine("|9900|9990|1|");
            contadorReg9900++; // Aqui por que ele esta se adicionando;
            contadorReg9900++;
            conteudoReg9001.AppendLine("|9900|9999|1|");
            conteudoReg9001.AppendLine("|9900|9900|" + contadorReg9900.ToString() + "|");

            totalReg9 += contadorReg9900 + 2; // +3 por que entra o registro 9999

            conteudoReg9001.AppendLine("|9990|" +
                totalReg9.ToString() + "|");


            int totalRegistro = totalReg0 + totalRegC + totalRegD + totalRegE + totalRegG + totalRegH + totalReg1 + totalReg9;

            conteudoReg9001.AppendLine("|9999|" +
                    Convert.ToString(totalRegistro) + "|");
            #endregion BLOCO 9
        }

        private void BlocoG(string acao)
        {
            if (acao == "abertura")
            {
                conteudoRegG001.AppendLine("|G001|" +
                    "1|");
                totalRegG++;
                registros.Add("G001|1");
            }

            if (acao == "fechamento")
            {
                conteudoRegG001.AppendLine("|G990|" +
                    "2|");
                totalRegG++;
                registros.Add("G990|1");
            }
        }

        private void BlocoH()
        {
            #region BLOCO H
            string dadosInformados = "1";
            // Bloco sem dados informados
            // 0 - Bloco com dados informados;
            // 1 - Bloco sem dados informados;
            foreach (var item in modeloDocFiscal)
            {
                if (item.modeloDocFiscal == "Inventario")
                    dadosInformados = "0";
            }

            decimal? totalInventario = (from n in entidade.produtosinventario
                                        where n.quantidade > 0 && n.codigo != ""
                                        && n.inventarionumero == numeroInventario
                                        && n.anoinventario == anoInventario
                                        select (decimal?)(n.customedio * n.quantidade)).Sum();
            if (totalInventario.GetValueOrDefault() == 0)
                dadosInformados = "1";


            conteudoRegH001.AppendLine("|H001|" +
               dadosInformados + "|");
            totalRegH++;
            registros.Add("H001|1");

            if (dadosInformados == "0")
            {
                conteudoRegH001.AppendLine("|H005|" +
                    string.Format("{0:ddMMyyyy}", dataInicial) + "|" +
                    string.Format("{0:N2}", totalInventario).Replace(".", "") + "|");
                totalRegH++;
                registros.Add("H005|1");

                var regH010 = from n in entidade.produtosinventario
                              where n.quantidade > 0 && n.codigo != ""
                              && n.inventarionumero == numeroInventario
                              && n.anoinventario == anoInventario
                              && n.quantidade > 0 && n.customedio > 0
                              select new { n.codigo, n.unidade, n.quantidade, n.customedio };

                foreach (var item in regH010)
                {
                    if (!codigos.Contains(item.codigo))
                        codigos.Add(item.codigo);

                    conteudoRegH001.AppendLine("|H010|" +
                        item.codigo + "|" +
                        item.unidade + "|" +
                        item.quantidade.ToString() + "|" +
                        item.customedio + "|" +
                        string.Format("{0:N2}", item.customedio * item.quantidade).Replace(".", "") + "|" +
                        "0|" +
                        "|" +
                        "|" +
                        item.codigo + "|");
                    contadorRegH010++;
                }
                registros.Add("H010|" + contadorRegH010.ToString());

                totalRegH += contadorRegH010;

            };

            totalRegH++;
            conteudoRegH001.AppendLine("|H990|" +
                totalRegH.ToString() + "|");
            registros.Add("H990|1");
            #endregion BLOCO H
        }


        private void BlocoD(string acao)
        {
            if (acao == "abertura")
            {
                conteudoRegD001.AppendLine("|D001|" +
                    "1|");
                totalRegD++;
                registros.Add("D001|1");
            }
            if (acao == "fechamento")
            {

                conteudoRegD001.AppendLine("|D990|" +
                    "2|");
                totalRegD++;
                registros.Add("D990|1");
            }

        }

        private void BlocoE(string acao, string movimento = "1")
        {
            if (acao == "abertura")
            {
                conteudoRegE001.AppendLine("|E001|" +
                   movimento + "|");
                totalRegE++;
                registros.Add("E001|1");
            }

            if (acao == "fechamento")
            {
                totalRegE++;
                conteudoRegE001.AppendLine("|E990|" +
                   totalRegE.ToString() + "|");
                registros.Add("E990|1");
            }
        }

        private void BlocoERegE100()
        {
            conteudoRegE001.AppendLine("|E100|" +
                string.Format("{0:ddMMyyyy}", dataInicial) + "|" +
                string.Format("{0:ddMMyyyy}", dataFinal) + "|");
            totalRegE++;
            registros.Add("E100|1");
        }

        private void BlocoERegE110()
        {
            icmsRecolher = totDebitos - totCreditos;
            if (icmsRecolher < 0)
                icmsRecolher = 0;
            saldoApurado = icmsRecolher;

            //saldoCredorTran = totCreditos;

            conteudoRegE001.AppendLine("|E110|" +
                totDebitos.ToString().Replace(".", "") + "|" +
                ajDebitos.ToString().Replace(".", "") + "|" +
                totAjDebitos.ToString().Replace(".", "") + "|" +
                estornoCreditos.ToString().Replace(".", "") + "|" +
                totCreditos.ToString().Replace(".", "") + "|" +
                ajCreditos.ToString().Replace(".", "") + "|" +
                totAjCreditos.ToString().Replace(".", "") + "|" +
                estornoDebitos.ToString().Replace(".", "") + "|" +
                saldoCredorAnt.ToString().Replace(".", "") + "|" +
                saldoApurado.ToString().Replace(".", "") + "|" +
                totDeducoes.ToString().Replace(".", "") + "|" +
                icmsRecolher.ToString().Replace(".", "") + "|" +
                saldoCredorTran.ToString().Replace(".", "") + "|" +
                extraApuracao.ToString().Replace(".", "") + "|"
                );
            totalRegE++;
            registros.Add("E110|1");

        }

        private void BlocoEReg116()
        {
            decimal vl_OR = icmsRecolher + extraApuracao;

            conteudoRegE001.AppendLine("|E116|" +
                "000|" +
                vl_OR.ToString().Replace(".", "") + "|" +
                string.Format("{0:ddMMyyyy}", dataInicial.Date) + "|" +
                "000|" + //COD_REC
                "|" + //NUM_PROC
                "|" + //IND_PROC
                "|" + //PROC
                "|" + // TXT_COMP
                string.Format("{0:MMyyyy}", dataInicial) + "|");
            totalRegE++;
            registros.Add("E116|1");
        }

        private void Bloco1(string acao, string movimento = "1")
        {
            if (dataInicial.Date >= Convert.ToDateTime("01/07/2012"))
            {
                movimento = "0";
            }

            if (acao == "abertura")
            {
                conteudoReg1001.AppendLine("|1001|" + movimento + "|");
                totalReg1++;
            }
            if (acao == "fechamento")
            {
                totalReg1++;
                registros.Add("1001|1");
                conteudoReg1001.AppendLine("|1990|" + totalReg1.ToString() + "|");
                registros.Add("1990|1");
            }
        }

        private void Bloco1Reg1010()
        {
            if (dataInicial.Date < Convert.ToDateTime("01/07/2012"))
                return;

            conteudoReg1001.AppendLine("|1010|N|N|N|N|N|N|N|N|N|");
            totalReg1++;
            registros.Add("1010|1");
        }


        private void Bloco0()
        {
            #region BLOCO 0

            #region BLOCO 0 Registro 0000
            conteudoReg0000.AppendLine("|0000|" + // 01 - REG
                codigoLayout + "|" + // 02 cod_ver
                codifoFinalidade + "|" +
                string.Format("{0:ddMMyyy}", dataInicial) + "|" +
                string.Format("{0:ddMMyyy}", dataFinal) + "|" +
                nomeEmpresa + "|" +
                cnpj + "|" +
                cpf + "|" +
                uf + "|" +
                ie + "|" +
                cod_mun + "|" +
                im + "|" +
                suframa + "|" +
                codigoPerfil + "|" +
                ind_ativ + "|");
            totalReg0++;
            registros.Add("0000|1");
            #endregion Bloco 0 Registro 0000
            #region BLOCO 0 Registro 0001
            conteudoReg0001.AppendLine("|0001|" +
                 "0|");
            contadorReg0001++;
            registros.Add("0001|" + contadorReg0001.ToString());

            #endregion Fim BLOCO 0 Registro 0001
            #region BLOCO 0 Registro 0005
            conteudoReg0005.AppendLine("|0005|" +
                fantasia + "|" +
                cep + "|" +
                end + "|" +
                num + "|" +
                comp + "|" +
                bairro + "|" +
                fone + "|" +
                fax + "|" +
                email + "|");
            contadorReg0005++;
            registros.Add("0005|" + contadorReg0005.ToString());
            #endregion
            #region BLOCO 0 Registro 0100 - Dados Contabilista
            conteudoReg0100.AppendLine("|0100|" +
                nomeContador + "|" +
                cpfContador + "|" +
                CRC + "|" +
                cnpjContador + "|" +
                cepContador + "|" +
                endContador + "|" +
                EndNumeroContador + "|" +
                complementoContador + "|" +
                bairroContador + "|" +
                foneContador + "|" +
                faxContador + "|" +
                emailContador + "|" +
                Funcoes.RetornaCodigoMunIBGE("M", cidade, estado) + "|");
            contadorReg0100++;
            registros.Add("0100|" + contadorReg0100.ToString());


            #endregion

            #region BLOCO 0 Registro 0150: Dados Participante

            foreach (var item in codigoParticipante)
            {
                try
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
                        if (dados == null)
                            throw new Exception("Fornecedor não foi encontrado: " + item.ToString());

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
                        if (dados == null)
                            throw new Exception("Cliente não foi encontrado: " + item.ToString());


                        codigo = dados.Codigo.ToString();
                        nome = dados.Nome ?? " ";
                        cnpjCli = dados.cnpj ?? " ";
                        cpfCli = dados.cpf ?? " ";
                        ieCli = dados.inscricao ?? " ";
                        enderecoCli = dados.endereco.Trim() ?? " ";
                        numeroCli = dados.numero ?? " ";
                        bairroCli = dados.bairro ?? " ";
                        cidadeCli = dados.cidade ?? " ";
                        estadoCli = dados.estado ?? " ";
                    }


                    conteudoReg0150.AppendLine("|0150|" +
                    codigo + item.Substring(0, 1) + "|" +
                    nome.Trim() + "|" +
                    "01058|" +
                    cnpjCli.Replace(".", "").Replace("-", "") + "|" +
                    cpfCli.Replace(".", "").Replace("-", "") + "|" +
                    ieCli.Replace(".", "").Replace("-", "").Trim().Replace("ISENTO", "") + "|" +
                    Funcoes.RetornaCodigoMunIBGE("M", cidadeCli, estadoCli) + "|" +
                    "|" + //SUFRAMA
                    enderecoCli.Trim().Replace("|", "") + "|" +
                    numeroCli.Trim() + "|" +
                    complementoCli.Trim() + "|" +
                    bairroCli.Trim() + "|");
                    contadorReg0150++;
                    //aqui
                }
                catch (Exception ex)
                {
                    MessageBox.Show("F-ornecedor ou C-liente nao encontrado " + item.ToString() + " Ex: " + ex.Message);
                }

            }

            if (codigoParticipante.Count > 0)
                registros.Add("0150|" + contadorReg0150.ToString());

            #endregion Bloco 0150

            #region BLOCO 0 Registro 0200: Tabela de identificação  do item (produto e serviços)

            // Foi criado essa lista para que nao se inclua 2 itens com o mesmo
            // codigo na tabela de identificacao. Exemplo se já tem o codigo de entrada
            // nao precisa ter novamente o codigo de saida. Se gerar 2 codigos iguais 
            // dar erro de validacao.

            Produtos dadosItem = new Produtos();
            //unidades.Clear();


            foreach (var registro in codigos)
            {

                try
                {
                    dadosItem.ProcurarCodigo(registro, GlbVariaveis.glb_filial, false);
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
                            parFilial.Value = registro;

                            cmd.ExecuteNonQuery();
                            conn.Close();
                            dadosItem.ProcurarCodigo(registro, GlbVariaveis.glb_filial, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exceção ao processar SP recriarCodigoProduto " + ex.Message);
                    }

                }


                if (!conteudoReg0200.ToString().Contains("|" + dadosItem.codigo + "|" + dadosItem.descricao + "|")) // string.IsNullOrEmpty(adicionar))
                {
                    string nbm = dadosItem.nbm;
                    string ncm = dadosItem.ncm;
                    string ncmespecie = dadosItem.ncmespecie;
                    string codigoBarras = dadosItem.codigoBarras;

                    if (ind_ativ != "0")
                        nbm = "";
                    ncm = ncm.PadRight(8, '0').Substring(0, 8);

                    if (ncmespecie == "SH")
                        ncmespecie = "";

                    if (codigoBarras.Length <= 6)
                        codigoBarras = "";

                    string codigoServico = "";
                    //if (dadosItem.situacaoInventario == "09")
                    //    codigoServico = dadosItem.codigo;

                    conteudoReg0200.AppendLine("|0200|" +
                           dadosItem.codigo + "|" +
                           dadosItem.descricao.Trim().Replace("|", "") + "|" +
                            codigoBarras + "|" +
                           "|" +
                           dadosItem.unidade + "|" +
                           dadosItem.situacaoInventario + "|" +
                           ncm.Trim() + "|" +
                           nbm.Trim() + "|" + // TIPI
                           ncmespecie.Trim() + "|" + // GEN
                           codigoServico + "|" +
                           dadosItem.icms + "|");
                    contadorReg0200++;
                    if (!unidades.Contains(dadosItem.unidade.ToUpper()))
                        unidades.Add(dadosItem.unidade.ToUpper());
                };

            };


            if (contadorReg0200 > 0)
                registros.Add("0200|" + contadorReg0200.ToString());

            if (unidades.Count > 0)
            {
                var lstUnidades = Produtos.Unidades();

                foreach (var item in unidades)
                {
                    var dados = (from n in lstUnidades
                                 where n.unidade == item
                                 select new { n.unidade, n.descricao }).FirstOrDefault();

                    if (dados == null)
                    {
                        conteudoReg0190.AppendLine("|0190|" +
                         item + "|" +
                        "INDEFINIDA" + "|");
                        contadorReg0190++;
                        // throw new Exception("Item com unidade não existente, unidade: " + item);
                    }
                    else
                    {
                        conteudoReg0190.AppendLine("|0190|" +
                          dados.unidade + "|" +
                          dados.descricao + "|");
                        contadorReg0190++;
                    }
                }
                registros.Add("0190|" + contadorReg0190.ToString());
            }

            #endregion

            #region  | BLOCO 0 Registro 0205: Alteração do ITEM
            //conteudoReg0205.AppendLine("|0205|" +
            //    "|" +
            //   "|" +
            //   "|" +
            //   "|");
            //contReg_0990++;            
            #endregion
            #region BLOCO 0 Registro 0990: Encerramento do Bloco 0
            totalReg0 += (contadorReg0001 + contadorReg0005 + contadorReg0150 + contadorReg0100 +
                contadorReg0190 + contadorReg0200) + 1;
            conteudoReg0990.AppendLine("|0990|" +
                totalReg0.ToString() + "|");
            registros.Add("0990|1");
            #endregion
            #endregion BLOCO 0;

        }
        //Aqu

    }

}

