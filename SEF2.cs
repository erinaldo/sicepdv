using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


/// Atribuir responsavel, cpfresponsavel

namespace SICEpdv
{
    /// <summary>
    /// Usando o ATO COTEPE 09/08 http://www.fazenda.gov.br/confaz/confaz/atos/atos_cotepe/2008/AC009_08.htm
    /// unidade de medidas oficiais: http://alice.desenvolvimento.gov.br/portalmdic/sitio/interna/interna.php?area=5&menu=1090&refr=605#Abreviaturas
    /// </summary>
    class SEF2 : dadosFiscaisEmpresa
    {
        public List<string> registros { get; set; }
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
        StringBuilder conteudoReg0025 = new StringBuilder();
        StringBuilder conteudoReg0030 = new StringBuilder();
        StringBuilder conteudoReg0100 = new StringBuilder();
        StringBuilder conteudoReg0150 = new StringBuilder();
        StringBuilder conteudoReg0175 = new StringBuilder();
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
        private int contadorReg0025 = 0;
        private int contadorReg0030 = 0;
        private int contadorReg0150 = 0;
        private int contadorReg0175 = 0;

        private int contadorReg0100 = 0;        
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
        public SEF2()
        {
            ObterDados();
            entidade = Conexao.CriarEntidade();
            registros = new List<string>();
            codigoLayout = "004";
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
            Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal, GlbVariaveis.glb_filial);
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
            NIRE = " ";
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

            // Variáveis 

        }

        public StringBuilder GerarSEF2()
        {

            #region BLOCO C
            BlocoC("abertura");
            BlocoCRegC100();
            BlocoCRegC300(); // Perfil B
            BlocoCRegC350(); // Perfil A
            BlocoCRegC400(); // Emitidas por ECF modelo doc fiscal 2D            
            BlocoC("fechamento");
            #endregion BLOCO C

            BlocoD("abertura"); BlocoD("fechamento");
            BlocoE("abertura"); BlocoE("fechamento");
            BlocoG("abertura"); BlocoG("fechamento");
            BlocoH();
            Bloco1("abertura"); Bloco1("fechamento");
            //// Bloco 0 -  nesta posição por que depois que processou as entradas e saída é que se obteu
            //// os códigos e as unidades utilizadas
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
            conteudo.Append(conteudoReg0025);
            conteudo.Append(conteudoReg0030);
            conteudo.Append(conteudoReg0100);
            conteudo.Append(conteudoReg0150);
            conteudo.Append(conteudoReg0175);            
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
                #region BLOCO C Registro C001: Abertura do Bloco C

                conteudoRegC001.AppendLine("|C001|" +
                    "0|");
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
            var dadosc400 = from n in entidade.r02                           
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            select n;

            var dadosRegistroC400 = dadosc400.ToList();

            //Registro PAI 
            foreach (var C400 in dadosRegistroC400)
            {
                conteudoRegC400.AppendLine("|C400|" +
                    "2D|" +
                    C400.modeloECF + "|" +
                    C400.fabricacaoECF + "|" +
                    C400.numeroECF + "|");
                contadorRegC400++;
            #endregion C400;


                #region BLOCO C Reigstro 405: Redução Z
                var dadosC405 = from n in entidade.r02
                                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                && n.fabricacaoECF == C400.fabricacaoECF
                                select n;

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
                    var regC420 = from n in entidade.r03
                                  where n.data == C405.data.Value && n.fabricacaoECF == C400.fabricacaoECF
                                  select n;
                    foreach (var item in regC420)
                    {
                        conteudoRegC400.AppendLine("|C420|" +
                            item.totalizadorParcial + "|" +
                            item.valoracumulado + "|" +
                            "|" +
                            "|");
                        contadorRegC420++;
                    }

                    #endregion C420;
                    #region BLOCO C Registro C460: Documento Fiscal emitor por ECF (codigo 02 e 2D)
                    var regC460 = from n in entidade.contdocs
                                  where n.data == C405.data.Value && n.ecffabricacao == C400.fabricacaoECF
                                  && n.total > 0
                                  select n;
                    foreach (var item in regC460)
                    {
                        var cod_sit = item.estornado == "N" ? "00" : "02";
                        conteudoRegC400.AppendLine("|C460|" +
                            "2D|" +
                            cod_sit + "|" +
                            item.ncupomfiscal + "|" +
                            string.Format("{0:ddMMyyyy}", C405.data.Value) + "|" +
                            item.Totalbruto + "|" +
                            "0|" +
                            "0|" +
                            item.ecfCPFCNPJconsumidor + "|" +
                            item.ecfConsumidor + "|");
                        contadorRegC460++;
                    }
                    #endregion C460
                    #region BLOCO C Registro C470: Itens do Documento Fiscal emitor por ECF (codigo 02 e 2D)
                    var regC470 = from n in entidade.blococregc470
                                  where n.data == C405.data.Value
                                  && n.ecffabricacao == C400.fabricacaoECF
                                  select n;
                    foreach (var item in regC470)
                    {
                        conteudoRegC400.AppendLine("|C470|" +
                            item.codigo + "|" +
                            item.quantidade + "|" +
                            "0|" +
                            item.unidade + "|" +
                            item.total + "|" +
                            "0" + item.tributacao + "|" +
                            item.cfop.Replace(".", "") + "|" +
                            item.icms + "|" +
                            "0|" + // VL_PIS
                            "0|"); // VL_COFINS     
                        contadorRegC470++;
                    }
                    #endregion C470
                    #region BLOCO C Registro C490: Registro analitico do movimento diário (codigo 02 e 2D)

                    var regC490 = from n in entidade.blococregc490
                                  where n.data == C405.data.Value && n.ecffabricacao == C400.fabricacaoECF
                                  select n;
                    foreach (var item in regC490)
                    {
                        conteudoRegC400.AppendLine("|C490|" +
                            "0" + item.tributacao + "|" +
                            item.cfop.Replace(".", "") + "|" +
                            item.icms + "|" +
                            item.total + "|" +
                            item.baseCalculoICMS.ToString().Replace(".", "") + "|" +
                            item.totalICMS + "|" +
                            "|");
                        contadorRegC490++;
                    }
                    #endregion C490
                }
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

                        if (!codigoParticipante.Contains(itemNF.codigofornecedor.ToString()))
                            codigoParticipante.Add("F" + itemNF.codigofornecedor.ToString());

                        string emitente = itemNF.Emitente == "P" ? "0" : "1";
                        string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";

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

                        var dadosItensAgr = from n in entidade.registro50entradas_agr
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
                                Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
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


        private void BlocoCRegC100()
        {
            if (!gerarRegC100Entrada)
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
                        var dadosEntradas = from n in entidade.moventradas
                                            where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                                            && n.modeloNF == item.modeloDocFiscal
                                            && n.Codigofilial == GlbVariaveis.glb_filial
                                            select n;
                        var RegistroEntrada = dadosEntradas.ToList();

                        foreach (var itemNF in RegistroEntrada)
                        {

                            if (!codigoParticipante.Contains(itemNF.codigofornecedor.ToString()))
                                codigoParticipante.Add("F" + itemNF.codigofornecedor.ToString());

                            string emitente = itemNF.Emitente == "P" ? "0" : "1";
                            string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";

                            conteudoRegC100.AppendLine("|C100|" +
                            "0|" + //IND_OPER 0:Entrada 1:Saida
                             emitente + "|" + // IND_EMIT
                            itemNF.codigofornecedor + "F|" +
                            itemNF.modeloNF + "|" +
                            situacaoNF + "|" +
                            itemNF.serie + "|" +
                            itemNF.NF + "|" +
                            "|" + // Retirado por que apresentou inconsistenciaitemNF.chave_nfe + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.DataEmissao) + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.dataEntrada) + "|" +
                            itemNF.ValorNota.ToString().Replace(".", "") + "|" +
                            itemNF.indicadorpagamento + "|" +
                            itemNF.descontos.ToString().Replace(".", "") + "|" +
                            0.ToString().Replace(".", "") + "|" +
                            itemNF.ValorProdutos.ToString().Replace(".", "") + "|" +
                            itemNF.tipofrete + "|" +
                            itemNF.Frete.ToString().Replace(".", "") + "|" +
                            itemNF.valorseguro.ToString().Replace(".", "") + "|" +
                            itemNF.Despesas.ToString().Replace(".", "") + "|" +
                            itemNF.BaseIcms.ToString().Replace(".", "") + "|" +
                            itemNF.Icms.ToString().Replace(".", "") + "|" +
                            itemNF.BaseIcmsSubst.ToString().Replace(".", "") + "|" +
                            itemNF.IcmsSubst.ToString().Replace(".", "") + "|" +
                            itemNF.IPI.ToString().Replace(".", "") + "|" +
                            itemNF.pis.ToString().Replace(".", "") + "|" +
                            itemNF.cofins.ToString().Replace(".", "") + "|" +
                            itemNF.pis_st.ToString().Replace(".", "") + "|" +
                            itemNF.cofins_st.ToString().Replace(".", "") + "|"); // VL_COFINS_ST
                            contadorRegC100++;

                            var dadosItens = from n in entidade.registro50entradas_itens
                                             where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                             && n.numero == itemNF.numero
                                             select n;
                            foreach (var registroItem in dadosItens)
                            {
                                if (!codigos.Contains(registroItem.codigo))
                                    codigos.Add(registroItem.codigo);

                                conteudoRegC100.AppendLine("|C170|" + //REG
                                    registroItem.sequencia.ToString() + "|" + //NUM_ITEM
                                    registroItem.codigo + "|" + // COD_ITEM
                                    registroItem.descricao + "|" + // DESCR_COMPL
                                    string.Format("{0:N5}", registroItem.quantidade).Replace(".", "") + "|" + //QTD
                                    registroItem.unidade + "|" + //? UNIDADE
                                    string.Format("{0:N2}", registroItem.custo).Replace(".", "") + "|" + // VL_ITEM
                                    "0" + "|" + // VL_DESC
                                    "0" + "|" + // IND_MOV
                                    registroItem.tributacao + "|" + //CST_ICMS
                                    registroItem.cfopentrada.Replace(".", "") + "|" +
                                    "|" + // COD_NAT  
                                    registroItem.bcicms.Value.ToString().Replace(".", "") + "|" + // VL_BC_ICMS
                                    registroItem.icmsentrada + "|" + // ALIQ_ICMS
                                    registroItem.toticms.ToString().Replace(".", "") + "|" + // VL_ICMS
                                    "0|" + // VL_BC_ICMS_ST
                                    "0|" + // ALIQ_ST
                                    "0|" + // VL_ICMS_ST
                                    "0|" + // IND_APUR
                                    "|" + // CST_IPI
                                    "|" + // COD_ENQ
                                    "0|" + // VL_BC_IPI
                                    "0|" + // ALIQ_IPI
                                    "0|" + // VL_IPI
                                    "|" + // CST_PIS
                                    "0|" + // VL_BC_PIS
                                    "0|" + // ALIQ_PIS
                                    "0|" + // QUANT_BC_PIS
                                    "0|" + // ALIQ_PIS
                                    "0|" + // VL_PIS
                                    "|" + // CST_COFINS
                                    "0|" + // VL_BC_COFINS
                                    "0|" + // ALIQ_COFINS
                                    "0|" + // QUANT_BC_COFINS
                                    "0|" + // ALIQ_COFINS
                                    "0|" + // VL_COFINS
                                    "|"); // COD_CTA 
                                contadorRegC170++;
                            }

                            var dadosItensAgr = from n in entidade.registro50entradas_agr
                                                where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                                && n.numero == itemNF.numero
                                                select n;
                            var dadosRegistrC190 = dadosItensAgr.ToList();

                            foreach (var regC190 in dadosRegistrC190)
                            {
                                conteudoRegC100.AppendLine("|C190|" +
                                    regC190.tributacao.PadLeft(3, '0').Substring(0, 3) + "|" +
                                    regC190.cfopentrada.Replace(".", "").Substring(0, 4) + "|" +
                                    Funcoes.FormatarZerosEsquerda(regC190.icmsentrada, 6, false) + "|" +
                                    regC190.totalNF.ToString().Replace(".", "") + "|" +
                                    regC190.bcicms.ToString().Replace(".", "") + "|" +
                                    regC190.toticms.ToString().Replace(".", "") + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                   regC190.ipiItem.ToString().Replace(".", "") + "|" +
                                    "|");
                                contadorRegC190++;
                            }

                        }
                    };
                    #endregion Entrada
                }

                #region Saida
                if (item.tipo == "Saida")
                {
                    if (gerarRegC100Saida)
                    {
                        var dadosSaida = from n in entidade.contnfsaida
                                         where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                         && n.modelodocfiscal == item.modeloDocFiscal
                                         && n.codigofilial == GlbVariaveis.glb_filial
                                         select n;
                        var RegistroSaida = dadosSaida.ToList();

                        foreach (var itemNF in RegistroSaida)
                        {
                            if (itemNF.codcliente > 0)
                            {
                                if (!codigoParticipante.Contains(itemNF.codcliente.ToString()))
                                    codigoParticipante.Add("C" + itemNF.codcliente.ToString());
                            }
                            if (itemNF.codfornecedor > 0)
                            {
                                if (!codigoParticipante.Contains(itemNF.codfornecedor.ToString()))
                                    codigoParticipante.Add("F" + itemNF.codfornecedor.ToString());
                            }

                            string emitente = "0";
                            string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";
                            Int32 codDestinatario = itemNF.codcliente > 0 ? itemNF.codcliente : itemNF.codfornecedor;

                            conteudoRegC100.AppendLine("|C100|" +
                            "1|" + //IND_OPER 0:Entrada 1:Saida
                             emitente + "|" + // IND_EMIT
                            codDestinatario.ToString() + "C|" +
                            itemNF.modelodocfiscal + "|" +
                            situacaoNF + "|" +
                            itemNF.serie + "|" +
                            itemNF.notafiscal + "|" +
                            itemNF.chave_nfe + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.data) + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.dataemissao) + "|" +
                            itemNF.totalNF.ToString().Replace(".", "") + "|" +
                            itemNF.indicadorpagamento + "|" +
                            itemNF.totaldesconto.ToString().Replace(".", "") + "|" +
                            0.ToString().Replace(".", "") + "|" +
                            itemNF.TotalProduto.ToString().Replace(".", "") + "|" +
                            itemNF.tipofrete + "|" +
                            itemNF.totalfrete.ToString().Replace(".", "") + "|" +
                            itemNF.totalseguro.ToString().Replace(".", "") + "|" +
                            itemNF.despesasacessorias.ToString().Replace(".", "") + "|" +
                            itemNF.basecalculo.ToString().Replace(".", "") + "|" +
                            itemNF.totalicms.ToString().Replace(".", "") + "|" +
                            itemNF.basecalculoICMSST.ToString().Replace(".", "") + "|" +
                            itemNF.totalICMSST.ToString().Replace(".", "") + "|" +
                            itemNF.totalipi.ToString().Replace(".", "") + "|" +
                            itemNF.pis.ToString().Replace(".", "") + "|" +
                            itemNF.cofins.ToString().Replace(".", "") + "|" +
                            0.ToString().Replace(".", "") + "|" +
                            0.ToString().Replace(".", "") + "|"); // VL_COFINS_ST
                            contadorRegC100++;
                            string notaFiscal = Convert.ToString(itemNF.notafiscal);

                            var dadosItens = from n in entidade.registro50saidas_itens
                                             where n.notafiscal == notaFiscal && n.serienf == itemNF.serie
                                             select n;


                            foreach (var registroItem in dadosItens)
                            {
                                if (!codigos.Contains(registroItem.codigo))
                                    codigos.Add(registroItem.codigo);

                                conteudoRegC100.AppendLine("|C170|" + //REG
                                    registroItem.nrcontrole.ToString() + "|" + //NUM_ITEM
                                    registroItem.codigo + "|" + // COD_ITEM
                                    registroItem.produto.Replace("|", "") + "|" + // DESCR_COMPL
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
                                    "|" + // VL_BC_ICMS_ST
                                    "|" + // ALIQ_ST
                                    "|" + // VL_ICMS_ST
                                    "0|" + // IND_APUR
                                    "|" + // CST_IPI
                                    "|" + // COD_ENQ
                                    "|" + // VL_BC_IPI
                                    "|" + // ALIQ_IPI
                                    "|" + // VL_IPI
                                    "|" + // CST_PIS
                                    "|" + // VL_BC_PIS
                                    "|" + // ALIQ_PIS
                                    "|" + // QUANT_BC_PIS
                                    "|" + // ALIQ_PIS
                                    "|" + // VL_PIS
                                    "|" + // CST_COFINS
                                    "|" + // VL_BC_COFINS
                                    "|" + // ALIQ_COFINS
                                    "|" + // QUANT_BC_COFINS
                                    "|" + // ALIQ_COFINS
                                    "|" + // VL_COFINS
                                    "|"); // COD_CTA                         
                                contadorRegC170++;
                            }

                            var dadosItensAgr = from n in entidade.registro50saida_agr
                                                where n.notafiscal == notaFiscal && n.serienf == itemNF.serie
                                                select n;
                            var dadosRegistroC190 = dadosItensAgr.ToList();

                            foreach (var regC190 in dadosRegistroC190)
                            {
                                conteudoRegC100.AppendLine("|C190|" +
                                    regC190.tributacao.PadLeft(3, '0').Substring(0, 3) + "|" +
                                    regC190.cfop.Replace(".", "").Substring(0, 4) + "|" +
                                    Funcoes.FormatarZerosEsquerda(regC190.icms, 6, false) + "|" +
                                    regC190.SUM_TOTAL_.ToString().Replace(".", "") + "|" +
                                    regC190.baseCalculoICMS.ToString().Replace(".", "") + "|" +
                                    regC190.totalicms.ToString().Replace(".", "") + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                    Funcoes.FormatarZerosEsquerda(0, 12, true) + "|" +
                                   0.ToString().Replace(".", "") + "|" +
                                    "|");
                                contadorRegC190++;
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
                conteudoRegC300.AppendLine("|C350|" +
                    item.SER + "|" +
                    item.SUB_SER + "|" +
                    item.NUM_DOC + "|" +
                    string.Format("{0:ddMMyyyy}", item.DT_DOC) + "|" +
                    item.CNPJ_CPF + "|" +
                    string.Format("{0:N2}", item.VL_MERC).Replace(".", "") + "|" +
                    string.Format("{0:N2}", item.VL_DOC).Replace(".", "") + "|" +
                    string.Format("{0:N2}", item.VL_DESC) + "|" +
                    item.VL_PIS + "|" +
                    item.VL_COFINS + "|" +
                    item.COD_CTA + "|");
                contadorRegC350++;
            #endregion C350
                #region BLOCO C Registro C370: Itens do Documento

                var dadosItens = from n in entidade.vendaarquivo
                                 where n.documento == item.NUM_DOC
                                 select n;

                foreach (var itens in dadosItens)
                {
                    if (!codigos.Contains(itens.codigo))
                        codigos.Add(itens.codigo);

                    conteudoRegC300.AppendLine("|C370|" +
                        itens.nrcontrole + "|" +
                        itens.codigo + "|" +
                        string.Format("{0:N3}", itens.quantidade) + "|" +
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
            if (totalInventario == 0)
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

        private void BlocoE(string acao)
        {
            if (acao == "abertura")
            {
                conteudoRegE001.AppendLine("|E001|" +
                    "1|");
                totalRegE++;
                registros.Add("E001|1");
            }

            if (acao == "fechamento")
            {
                conteudoRegE001.AppendLine("|E990|" +
                    "2|");
                totalRegE++;
                registros.Add("E990|1");
            }
        }


        private void Bloco1(string acao)
        {
            if (acao == "abertura")
            {
                conteudoReg1001.AppendLine("|1001|" +
                    "1|");
                totalReg1++;
            }
            if (acao == "fechamento")
            {
                registros.Add("1001|1");
                conteudoReg1001.AppendLine("|1990|" +
                    "2|");
                totalReg1++;
                registros.Add("1990|1");
            }
        }

        private void Bloco0()
        {
            #region BLOCO 0
            
            #region BLOCO 0 Registro 0000
            conteudoReg0000.AppendLine("|0000|" + // 01 - REG
                "LFPD|"+
                string.Format("{0:ddMMyyy}", dataInicial) + "|" +
                string.Format("{0:ddMMyyy}", dataFinal) + "|" +                            
                nomeEmpresa + "|" +
                cnpj + "|" +
                uf + "|" +
                ie + "|" +
                cod_mun + "|" +
                im + "|" +
                "|"+
                suframa +"|"+
                codigoLayout +"|"+                               
                codifoFinalidade + "|" +
                "0|BRASIL|"+
                fantasia+"|"+
                NIRE +"|"+ // NIRE                                               
                cpf + "|" +
                "|");
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
                responsavel + "|" +
                "000|" + //(?)
                cpfresponsavel + "|"+
                cep + "|" +
                end + "|" +
                num + "|" +
                comp + "|" +
                bairro + "|" +
                "|"+
                "|"+
                fone + "|" +
                fax + "|" +
                email + "|");
            contadorReg0005++;
            registros.Add("0005|" + contadorReg0005.ToString());
            #endregion
            #region BLOCO 0 Registro 0025: Benefício fiscal
            conteudoReg0025.AppendLine("|0025|" +
                "|" +
                "|");
            contadorReg0025++;
            registros.Add("|0025|" + contadorReg0025.ToString());
            #endregion
            #region BLOCO 0 Registro 0030: Perfil do contribuinte
            conteudoReg0030.AppendLine("|0030|" +
                "0|" + //IND_ED
                "0|" + //IND_ARQ
                "9|" + //PRF_ISS 
                "0|" + //PRF_ICMS
                "N|" + //PRF_RIDF
                "N|" + //PRF_RUDF
                "N|" + //PRF_LCM
                "N|" + //PRF_RV
                "N|" + //PRF_RI
                "0|" + //IND_EC
                "N|" + //IND_ISS
                "N|" + //IND_RT
                "S|" + //IND_ICMS
                "N|" + //IND_ST
                "N|" + //IND_AT
                "N|" + //IND_IPI
                "N|" //IND_RI
                );
            contadorReg0030++;
            registros.Add("|0030|" + contadorReg0030.ToString());


#endregion
            #region BLOCO 0 Registro 0100 - Dados Contabilista
            conteudoReg0100.AppendLine("|0100|" +
                nomeContador + "|" +
                "900|" +
                cnpjContador + "|" +
                cpfContador + "|" +
                CRC + "|" +
                cepContador + "|" +
                endContador + "|" +
                EndNumeroContador + "|" +
                complementoContador + "|" +
                bairroContador + "|" +
                uf + "|" +
                Funcoes.RetornaCodigoMunIBGE("M", cidade, estado) + "|" +
                "|"+
                foneContador + "|" +
                faxContador + "|" +
                emailContador + "|");                
            contadorReg0100++;
            registros.Add("|0100|" + contadorReg0100.ToString());


            #endregion
            #region BLOCO 0 Registro 0150: Dados Participante

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
                string cepCli = "";
                string foneCli = "";
                string faxcli = "";

                if (item.Substring(0, 1) == "F")
                {
                    Int32 codFor = Convert.ToInt32(item.Substring(1, item.Length - 1));

                    var dados = (from n in entidade.fornecedores
                                 where n.Codigo == codFor
                                 select new
                                 {
                                     n.Codigo,
                                     n.empresa,
                                     n.TELEFONE,
                                     n.FAX,
                                     n.CGC,
                                     n.CPF,
                                     n.INSCRICAO,
                                     n.ENDERECO,
                                     n.numero,
                                     n.BAIRRO,
                                     n.CIDADE,
                                     n.CEP,
                                     n.ESTADO
                                 }).First();

                    codigo = dados.Codigo.ToString();
                    nome = dados.empresa;
                    cnpjCli = dados.CGC;
                    cpfCli = dados.CPF;
                    ieCli = dados.INSCRICAO;
                    enderecoCli = dados.ENDERECO.Trim();
                    numeroCli = dados.numero;
                    bairroCli = dados.BAIRRO;
                    cidadeCli = dados.CIDADE;
                    estadoCli = dados.ESTADO;
                    cepCli = dados.CEP;
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
                                     n.telefone,
                                     n.fax,
                                     n.cnpj,
                                     n.cpf,
                                     n.inscricao,
                                     n.endereco,
                                     n.numero,
                                     n.bairro,
                                     n.cidade,
                                     n.cep,
                                     n.estado
                                 }).First();

                    codigo = dados.Codigo.ToString();
                    nome = dados.Nome;
                    cnpjCli = dados.cnpj;
                    cpfCli = dados.cpf;
                    ieCli = dados.inscricao;
                    enderecoCli = dados.endereco.Trim();
                    numeroCli = dados.numero;
                    bairroCli = dados.bairro;
                    cidadeCli = dados.cidade;
                    estadoCli = dados.estado;
                    cepCli = dados.cep;
                }


                conteudoReg0150.AppendLine("|0150|" +
                codigo + item.Substring(0, 1) + "|" +
                nome + "|" +
                "01058|" +
                cnpjCli + "|" +
                cpfCli + "|" +
                "|"+
                estadoCli +"|"+
                ieCli + "|" +
                "|"+
                Funcoes.RetornaCodigoMunIBGE("M", cidadeCli, estadoCli) + "|" +
                "|"+
                "|" );
                contadorReg0150++;
                #region Registro 0175 Endereço participante
                conteudoReg0175.AppendLine("|0175|" +
                    cepCli + "|" +
                    enderecoCli + "|" +
                    numeroCli + "|" +
                    complementoCli + "|" +
                    bairroCli + "|" +
                    "|" +
                    "|" +
                    foneCli + "|" +
                    faxcli + "|");                                        
                contadorReg0175++;
                #endregion
            }

            if (codigoParticipante.Count > 0)
            {
                registros.Add("|0150|" + contadorReg0150.ToString());
                registros.Add("|0175|" + contadorReg0175.ToString());
            }

            #endregion Bloco 0150

            #region BLOCO 0 Registro 0200: Tabela de identificação  do item (produto e serviços)

            // Foi criado essa lista para que nao se inclua 2 itens com o mesmo
            // codigo na tabela de identificacao. Exemplo se já tem o codigo de entrada
            // nao precisa ter novamente o codigo de saida. Se gerar 2 codigos iguais 
            // dar erro de validacao.
            List<string> unidades = new List<string>();

            foreach (var registro in codigos)
            {
                var dados = (from n in entidade.produtos
                             where n.codigo == registro
                             select new
                             {
                                 n.codigo,
                                 n.descricao,
                                 n.codigobarras,
                                 n.unidade,
                                 n.situacaoinventario,
                                 n.ncm,
                                 n.nbm,
                                 n.ncmespecie,
                                 n.icms
                             }).FirstOrDefault();

                if (dados == null)
                    throw new Exception("Código não está cadastrado nos produtos: " + registro);

                if (!unidades.Contains(dados.unidade))
                    unidades.Add(dados.unidade);

                string codigoServico = "";
                if (dados.situacaoinventario == "09")
                    codigoServico = dados.codigo;

                conteudoReg0200.AppendLine("|0200|" +
                       dados.codigo + "|" +
                       dados.descricao.Trim() + "|" +
                       dados.codigobarras + "|" +                       
                       dados.icms + "|");
                contadorReg0200++;
            };

            if (contadorReg0200 > 0)
                registros.Add("0200|" + contadorReg0200.ToString());

           

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
            totalReg0 += (contadorReg0001 + contadorReg0005 + contadorReg0025 + contadorReg0030 + contadorReg0150 + contadorReg0175 +  contadorReg0100 +
                contadorReg0200) + 1;
            conteudoReg0990.AppendLine("|0990|" +
                totalReg0.ToString() + "|");
            registros.Add("0990|1");
            #endregion
            #endregion BLOCO 0;
        }

    }

}
