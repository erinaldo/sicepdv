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
    class SPEDPisCofins : dadosFiscaisEmpresa
    {
        public List<string> registros { get; set; }
        List<string> unidades = new List<string>();
        private siceEntities entidade;
        public List<ModeloDocFiscal> modeloDocFiscal { get; set; }
        public List<string> filiaisEFD { get; set; }
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
        // Informações Bloco 0 REg 0110
        public string cod_inc_trib { get; set; }
        public string ind_apro_cred { get; set; }
        public string cod_tipo_cont { get; set; }
        public string ind_reg_cum { get; set; }

        // Opções de exportação de registro
        public bool gerarRegC100Entrada { get; set; }
        public bool gerarRegC100Saida { get; set; }
        public bool gerarRegC300 { get; set; }
        public bool gerarRegC350 { get; set; }
        public bool gerarRegC400 { get; set; }
        public bool gerarRegC490 { get; set; }

        public bool gerarRegC500 { get; set; }

        List<StruCodigos> codigos = new List<StruCodigos>();
        List<string> codigoParticipante = new List<string>();
        List<string> codigoInfoCompNF = new List<string>();
        List<StruM410> listaM410 = new List<StruM410>();
        List<StruM410> listaM810 = new List<StruM410>();

        // String do conteúdo dos arquivos: Na criação da varíavel do conteúdo 
        // seguir a sequência do layout SPED e adicionar na variável conteúdo
        // Para ficar seguindo o layout 
        public StringBuilder conteudo = new StringBuilder();
        StringBuilder conteudoReg0000 = new StringBuilder();
        StringBuilder conteudoReg0001 = new StringBuilder();        
        StringBuilder conteudoReg0100 = new StringBuilder();
        StringBuilder conteudoReg0110 = new StringBuilder();
        StringBuilder conteudoReg0140 = new StringBuilder();
        StringBuilder conteudoReg0150 = new StringBuilder();
        StringBuilder conteudoReg0190 = new StringBuilder();
        StringBuilder conteudoReg0200 = new StringBuilder();
        StringBuilder conteudoReg0450 = new StringBuilder();
        StringBuilder conteudoReg0990 = new StringBuilder();


        StringBuilder conteudoBlocoA = new StringBuilder();

        StringBuilder conteudoRegC001 = new StringBuilder();
        StringBuilder conteudoRegC100 = new StringBuilder();
        StringBuilder conteudoRegC380 = new StringBuilder();
        StringBuilder conteudoRegC400 = new StringBuilder();
        StringBuilder conteudoRegC490 = new StringBuilder();
        StringBuilder conteudoRegC500 = new StringBuilder();

        StringBuilder conteudoRegC990 = new StringBuilder();

        StringBuilder conteudoRegD001 = new StringBuilder();
        

        StringBuilder conteudoBlocoF = new StringBuilder();
        

        StringBuilder conteudoBlocoM = new StringBuilder();

        StringBuilder conteudoReg1001 = new StringBuilder();

        StringBuilder conteudoReg9001 = new StringBuilder();

        // Apuração Bloco E 
        private decimal totDebitos = 0, ajDebitos = 0, totAjDebitos = 0, estornoCreditos = 0,
      totCreditos = 0, ajCreditos = 0, totAjCreditos = 0, estornoDebitos = 0, saldoCredorAnt = 0,
      saldoApurado = 0, totDeducoes = 0, icmsRecolher = 0, saldoCredorTran = 0, extraApuracao = 0;

        // Contadores de REgistros
        private int totalReg0 = 0;
        private int totalRegA = 0;
        private int totalRegC = 0;
        private int totalRegD = 0;
        private int totalRegF = 0;        
        private int totalRegM = 0;
        private int totalReg1 = 0; // Bloco 1
        private int totalReg9 = 0;

        // Registro BLOCO 0
        private int contadorReg0001 = 0;        
        private int contadorReg0150 = 0;
        private int contadorReg0100 = 0;
        private int contadorReg0110 = 0;
        private int contadorReg0140 = 0;
        private int contadorReg0190 = 0;
        private int contadorReg0200 = 0;
        private int contadorReg0450 = 0;

        // Registros BLOCO C
        private int contadorRegC010 = 0;
        private int contadorRegC100 = 0;
        private int contadorRegC110 = 0;
        private int contadorRegC170 = 0;
        private int contadorRegC190 = 0;
        private int contadorRegC380 = 0;
        private int contadorRegC381 = 0;
        private int contadorRegC385 = 0;
        private int contadorRegC400 = 0;
        private int contadorRegC405 = 0;        
        private int contadorRegC481 = 0;
        private int contadorRegC485 = 0;
        private int contadorRegC490 = 0;
        private int contadorRegC491 = 0;
        private int contadorRegC495 = 0;

        private int contadorRegC500 = 0;
        private int contadorRegC501 = 0;
        private int contadorRegC505 = 0;

        //Registros BLOCO M
        private int contadorRegM410 = 0;
        private int contadorRegM810 = 0;



        // Criar entradas
        //Construtor inicializando variáveis na criação do objeto
        public SPEDPisCofins()
        {
            ObterDados();
            entidade = Conexao.CriarEntidade();
            registros = new List<string>();
            filiaisEFD = new List<string>();
            codigoLayout = "002";
            codifoFinalidade = "0";
            codigoPerfil = "A";
            // 0110
             cod_inc_trib = "3";
             ind_apro_cred = "1";
             cod_tipo_cont = "1";
             ind_reg_cum = "9";

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

        public StringBuilder GerarEFD()
        {
            Funcoes.CriarTabelaTmp("venda", dataInicial, dataFinal,"00000");
            #region BLOCO A
            BlocoA("abertura"); BlocoA("fechamento");
            #endregion

            #region BLOCO C
            BlocoC("abertura");

            foreach (var item in filiaisEFD)
            {
                BlocoCRegC010(item);
                BlocoCRegC100(item);
                //BlocoCRegC300(); // Perfil B
                
                //(FUNCIONANDO - PELOS Registro R01 dados do ECF)
                BlocoCRegC400(item); // Emitidas por ECF modelo doc fiscal 2D pegando dados do ECF 
                
                // Apuracao pela consolidacao a apuracao pelo C400 está funcionando perfeitamente
                // Foi feito pela consolidacao por que se houver falha no fechamento do 
                // ECF a apuracao pela consolidacao independe do ECF
                BlocoCRegC380(item); // Modelo 02 lancamento PED
                
                BlocoCRegC490(item); // Emitidas por ECF consolidadas por itens
                
            }
            BlocoC("fechamento");
            #endregion BLOCO C


            #region BLOCO D
            BlocoD("abertura"); BlocoD("fechamento");
            #endregion

            #region BLOCO F
            BlocoF("abertura"); BlocoF("fechamento");
            #endregion

            #region BLOCO M
            BlocoM("abertura");
            BlocoMRegM410();
            BlocoMRegM810();
            BlocoM("fechamento");
            #endregion
          
            Bloco1("abertura"); Bloco1("fechamento");
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
            conteudo.Append(conteudoReg0100);
            conteudo.Append(conteudoReg0110);
            conteudo.Append(conteudoReg0140);
            conteudo.Append(conteudoReg0150);
            conteudo.Append(conteudoReg0190);
            conteudo.Append(conteudoReg0200);
            conteudo.Append(conteudoReg0450);
            conteudo.Append(conteudoReg0990);

            conteudo.Append(conteudoBlocoA);

            conteudo.Append(conteudoRegC001);
            conteudo.Append(conteudoRegC100);            
            conteudo.Append(conteudoRegC400);            
            conteudo.Append(conteudoRegC990);

            conteudo.Append(conteudoRegD001);
            

            conteudo.Append(conteudoBlocoF);

            conteudo.Append(conteudoBlocoM);                    

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


                if (!gerarRegC100Entrada && !gerarRegC100Saida)
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
                totalRegC += (contadorRegC010 + contadorRegC100 + contadorRegC110 + contadorRegC170 + contadorRegC190  + contadorRegC380 + contadorRegC381 + contadorRegC385 +  contadorRegC400 +
                contadorRegC405 + contadorRegC481 + contadorRegC485 + contadorRegC490 + contadorRegC491 + contadorRegC495 + contadorRegC500 + contadorRegC501 + contadorRegC505) + 1;

                registros.Add("C010|" +
                    contadorRegC010.ToString());

                if (contadorRegC100 > 0)
                {
                    registros.Add("C100|" + contadorRegC100.ToString());
                    if (contadorRegC110>0)
                    {
                        registros.Add("C110|" + contadorRegC110.ToString());
                    }


                    registros.Add("C170|" + contadorRegC170.ToString());

                }
                if (contadorRegC380 > 0)
                {
                    registros.Add("C380|" + contadorRegC380.ToString());
                    registros.Add("C381|" + contadorRegC381.ToString());
                    registros.Add("C385|" + contadorRegC385.ToString());
                }

                if (contadorRegC400 > 0)
                {
                    registros.Add("C400|" + contadorRegC400.ToString());
                    registros.Add("C405|" + contadorRegC405.ToString());
                    registros.Add("C481|" + contadorRegC481.ToString());
                    registros.Add("C485|" + contadorRegC485.ToString());
                }

                if (contadorRegC490 > 0)
                {
                    registros.Add("C490|" + contadorRegC490.ToString());
                    registros.Add("C491|" + contadorRegC491.ToString());
                    registros.Add("C495|" + contadorRegC495.ToString());

                }

                conteudoRegC990.AppendLine("|C990|" +
                    totalRegC.ToString() + "|");
                registros.Add("C990|1");
                #endregion Registro C990
            }
        }

        private void BlocoCRegC010(string filial)
        {
                string indicadorMovimento = "1";
                // Verificando se há movimento
                var dadosSaida = from n in Conexao.CriarEntidade().contnfsaida
                                 where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                 && (n.modelodocfiscal == "55" || n.modelodocfiscal == "01")
                                 && n.codigofilial == filial
                                 select n;
                if (dadosSaida.Count() > 0)
                    indicadorMovimento = "0";

                var dadosEntradas = from n in Conexao.CriarEntidade().moventradas
                                    where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                                    && (n.modeloNF == "55" || n.modeloNF == "01")
                                    && n.Codigofilial == filial
                                    select n;
                if (dadosEntradas.Count() > 0)
                    indicadorMovimento = "0";

                var movimentoECF = (from n in Conexao.CriarEntidade().contdocs
                                    where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                    && n.modeloDOCFiscal == "2D"
                                    && n.CodigoFilial == filial
                                    select new { n.data, n.modeloDOCFiscal });
                if (movimentoECF.Count() > 0)
                    indicadorMovimento = "0";


                if (!gerarRegC100Entrada && !gerarRegC100Saida)
                    indicadorMovimento = "1";

                #region BLOCO C Registro C010: Abertura do movimento


                if (indicadorMovimento == "0")
                {
                    var dadosFilial = (from n in entidade.filiais                                      
                                      where n.CodigoFilial == filial
                                      select new { n.fantasia, n.cnpj,n.estado,n.inscricao,n.cidade,n.inscricaomunicipal }).First();


                    #region BLOCO 0 Registro 0140 - Cadastro de Estabelecimento
                    conteudoReg0140.AppendLine("|0140|" +
                        new string(' ', 60) + "|" +
                        dadosFilial.fantasia+ "|" +
                        dadosFilial.cnpj + "|" +
                        dadosFilial.estado + "|" +
                        dadosFilial.inscricao.Trim() + "|" +
                        Funcoes.RetornaCodigoMunIBGE("M", dadosFilial.cidade, dadosFilial.estado) + "|" +
                        dadosFilial.inscricaomunicipal + "|" +
                         "|");
                    contadorReg0140++;
                   
                    #endregion

                    #region BLOCO C Registro C010 Identificação do estabelecimento
                    conteudoRegC100.AppendLine("|C010|" +
                        dadosFilial.cnpj + "|" +
                        "|");
                    contadorRegC010++;
                    
                    #endregion Registro C010
                }                
                #endregion C001

        }
        // REGISTRO C380: NOTA FISCAL DE VENDA A CONSUMIOR Código 02 - Consolidação de doc emitidos
        private void BlocoCRegC380(string filial)
        {

            var dataDocInicial = (from n in entidade.contdocs
                                  where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                  && n.CodigoFilial == filial
                                  && n.modeloDOCFiscal == "02"
                                  && n.serienf == "D"
                                  select n.data).Min();
            if (dataDocInicial == null)
                return;

            var dataDocFinal = (from n in entidade.contdocs
                                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                && n.CodigoFilial == filial
                                select n.data).Max();

            var docInicial = (from n in entidade.contdocs
                                  where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                  && n.CodigoFilial == filial
                                  && n.modeloDOCFiscal == "02"
                                  && n.serienf == "D"
                                  select n.nrnotafiscal).Min();
            
            var docFinal = (from n in entidade.contdocs
                                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                && n.CodigoFilial == filial
                                && n.modeloDOCFiscal=="02"
                                && n.serienf == "D"
                                select n.nrnotafiscal).Max();

            var totalDocs = (from n in entidade.contdocs
                             where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                             && n.CodigoFilial == filial
                             && n.modeloDOCFiscal == "02"
                             && n.serienf == "D"
                             select n.total).Sum();
            
            conteudoRegC380.AppendLine("|C380|" +
                "02|"+
                string.Format("{0:ddMMyyyy}", dataDocInicial.Value) + "|" +
                string.Format("{0:ddMMyyyy}", dataDocFinal.Value) + "|" +
                docInicial.ToString()+"|"+
                docFinal.ToString()+"|" +
                totalDocs.Value.ToString().Replace(".", "") + "|" +
                "0|");
            contadorRegC380++;

            var dadosC381 = from n in entidade.blococregc381_pis
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            && n.codigofilial == filial
                            && n.modelodocfiscal == "02"
                            && n.total > 0
                            select n;
            var regC381 = dadosC381.ToList().Distinct();

            foreach (var item in regC381)
            {
                string cod = (from n in codigos
                              where n.codigo == item.codigo
                              //&& n.filial == filial
                              select n.codigo).FirstOrDefault();

                if (cod == null || string.IsNullOrEmpty(cod))
                {
                    StruCodigos novoCod = new StruCodigos()
                    {
                        codigo = item.codigo,
                        filial = filial
                    };
                    codigos.Add(novoCod);
                }


                if (!unidades.Contains(item.unidade))
                    unidades.Add(item.unidade);

                conteudoRegC380.AppendLine("|C381|" +
                    item.cstpis + "|" +
                    item.codigo + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.pis.ToString().Replace(".", "") + "|" +
                    "|" +
                    "|" +
                     item.totalPIS.ToString().Replace(".", "") + "|" +
                     "|");
                contadorRegC381++;
            };

            var dadosC385 = from n in entidade.blococregc385_cofins
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            && n.codigofilial == filial
                            && n.modelodocfiscal == "02"
                            && n.total > 0
                            select n;
            var regC385 = dadosC385.ToList().Distinct();

            foreach (var item in regC385)
            {
                conteudoRegC380.AppendLine("|C385|" +
                    item.cstpis + "|" +
                    item.codigo + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.cofins.ToString().Replace(".", "") + "|" +
                    "|" +
                    "|" +
                     item.totalCOFINS.ToString().Replace(".", "") + "|" +
                     "|");
                contadorRegC385++;
            }

            conteudoRegC100.Append(conteudoRegC380);
            conteudoRegC380.Clear();
        }


        private void BlocoCRegC400(string filial)
        {
            if (!gerarRegC400)
                return;

            #region BLOCO C Registro C400 -->C405--> Equipamento ECF (Codigo 02 e 2D)

            #region BLOCO C Registro C400: Equipamento ECF (codigo 02 e 2D)
            //var dadosc400 = (from n in entidade.r02                            
            //                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
            //                && n.codigofilial == GlbVariaveis.glb_filial                            
            //                select n);

            //var dadosRegistroC400 = dadosc400.ToList();

            var ecfs = (from n in entidade.r02
                        where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                        && n.codigofilial == filial
                        select new { n.fabricacaoECF, n.numeroECF }).Distinct().ToList();
           
            //Registro PAI 
            foreach (var ecf in ecfs)
            {
                var C400 = (from n in entidade.r02
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            && n.fabricacaoECF == ecf.fabricacaoECF
                            && n.numeroECF == ecf.numeroECF
                            && n.codigofilial == filial
                            select n).First();
                 if (conteudoRegC400.ToString().Contains(C400.fabricacaoECF))
                     return;

                conteudoRegC400.AppendLine("|C400|" +
                    "2D|" +
                    C400.modeloECF.Trim() + "|" +
                    C400.fabricacaoECF + "|" +
                    C400.numeroECF + "|");
                contadorRegC400++;
            #endregion C400;


                #region BLOCO C Reigstro 405: Redução Z
                var dadosC405 = from n in entidade.r02
                                where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                && n.fabricacaoECF == C400.fabricacaoECF
                                && n.numeroECF == C400.numeroECF
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

                    #region BLOCO C Registro C481: RESUMO DIÁRIO DE DOCUMENTOS EMITIDOS POR ECF – PIS/PASEP (CÓDIGOS 02 e 2D).
                    var dadosC481 = from n in entidade.blococregc481_pis
                                    where n.data == C405.data.Value
                                    && n.ecffabricacao == C405.fabricacaoECF
                                    && n.ecfnumero == C405.numeroECF
                                    && n.codigofilial == filial
                                    && n.modelodocfiscal == "2D"
                                    && n.total > 0
                                    select n;

                    var regC481 = dadosC481.ToList().Distinct();

                    foreach (var item in regC481)
                    {
                        string cod = (from n in codigos
                                      where n.codigo == item.codigo
                                      // && n.filial == filial
                                      select n.codigo).FirstOrDefault();

                        if (cod == null || string.IsNullOrEmpty(cod))
                        {
                            StruCodigos novoCod = new StruCodigos()
                            {
                                codigo = item.codigo,
                                filial = filial
                            };
                            codigos.Add(novoCod);
                        }


                        if (!unidades.Contains(item.unidade))
                            unidades.Add(item.unidade);

                        conteudoRegC400.AppendLine("|C481|" +
                            item.cstpis + "|" +
                            item.total.ToString().Replace(".", "") + "|" +
                            item.total.ToString().Replace(".", "") + "|" +
                            item.pis.ToString().Replace(".", "") + "|" +
                            "|" +
                            "|" +
                            item.totalPIS.ToString().Replace(".", "") + "|" +
                            item.codigo + "|" +
                            "|");
                        contadorRegC481++;

                        VerificarSuspensaoPisCofins(filial, item.codigo, item.cstpis, item.cstcofins, item.total.Value);
                    }
                    #endregion C481  RESUMO DIÁRIO DE DOCUMENTOS EMITIDOS POR ECF – COFINS (CÓDIGOS 02 e 2D).
                    #region BLOCO C Registro C485:
                    var dadosC485 = from n in entidade.blococregc485_cofins
                                    where n.data == C405.data 
                                    && n.ecffabricacao == C405.fabricacaoECF
                                    && n.ecfnumero == C405.numeroECF
                                    && n.codigofilial == filial
                                    && n.modelodocfiscal == "2D"
                                    && n.total > 0
                                    select n;

                    var regC485 = dadosC485.ToList().Distinct();
                    foreach (var item in regC485)
                    {

                        conteudoRegC400.AppendLine("|C485|" +
                            item.cstcofins + "|" +
                            item.total.ToString().Replace(".", "") + "|" +
                            item.total.ToString().Replace(".", "") + "|" +
                            item.cofins.ToString().Replace(".", "") + "|" +
                            "|" +
                            "|" +
                            item.totalCOFINS.ToString().Replace(".", "") + "|" +
                            item.codigo + "|" +
                            "|");
                        contadorRegC485++;
                    }
                    #endregion C485


                } // C405
            }


         
            #endregion Registro C400
            conteudoRegC100.Append(conteudoRegC400);
            conteudoRegC400.Clear();
        }

        //Nota Fiscal Conta Energia Eletrica - 06 - A´gua canalizada 28 - Fornecimento de Gás - 29

        private void BlocoCRegC490(string filial)
        {
            if (!gerarRegC490)
                return;

            var dataDocInicial = (from n in entidade.contdocs
                        where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                        && n.CodigoFilial == filial
                        && n.modeloDOCFiscal == "2D"
                        select n.data).Min();
            
            if (dataDocInicial == null)
                return;

            var dataDocFinal = (from n in entidade.contdocs
                                  where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                  && n.CodigoFilial == filial
                                  select n.data).Max();
            conteudoRegC490.AppendLine("|C490|" +
                string.Format("{0:ddMMyyyy}", dataDocInicial.Value) + "|" +
                string.Format("{0:ddMMyyyy}", dataDocFinal.Value) + "|" +
                "2D|");
            contadorRegC490++;            

            var dadosC491 = from n in Conexao.CriarEntidade().blococregc491_pis
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            && n.codigofilial == filial
                            && n.modelodocfiscal == "2D"
                            && n.total > 0
                            select new { n.codigo, n.descricao,  n.cstpis, n.cstcofins, n.pis,n.totalPIS, n.unidade, n.total, n.cfop };

            var regC491 = dadosC491.ToList();


            foreach (var item in regC491)
            {
                
                string cod = (from n in codigos
                              where n.codigo == item.codigo
                             // && n.filial == filial
                              select n.codigo).FirstOrDefault();

                if (cod == null || string.IsNullOrEmpty(cod))
                {
                    StruCodigos novoCod = new StruCodigos()
                    {
                        codigo = item.codigo,
                        filial = filial
                    };
                    codigos.Add(novoCod);
                }


                if (!unidades.Contains(item.unidade))
                    unidades.Add(item.unidade);

                conteudoRegC490.AppendLine("|C491|" +
                    item.codigo + "|" +
                    item.cstpis + "|" +
                    item.cfop.Replace(".","") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.pis.ToString().Replace(".", "") + "|" +
                    "|" +
                    "|" +
                     item.totalPIS.ToString().Replace(".", "") + "|" +
                     "|");

                VerificarSuspensaoPisCofins(filial, item.codigo, item.cstpis,item.cstcofins,item.total.Value);


                contadorRegC491++;
            };
            var dadosC495 = from n in Conexao.CriarEntidade().blococregc495_cofins
                            where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                            && n.codigofilial == filial
                            && n.modelodocfiscal == "2D"
                            && n.total > 0
                            select new { n.codigo, n.descricao, n.cstcofins, n.cofins, n.totalCOFINS, n.unidade, n.total, n.cfop };
            
            var regC495 = dadosC495.ToList();

            foreach (var item in regC495)
            {
                conteudoRegC490.AppendLine("|C495|" +
                    item.codigo + "|" +
                    item.cstcofins + "|" +
                    item.cfop.Replace(".","") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.total.ToString().Replace(".", "") + "|" +
                    item.cofins.ToString().Replace(".", "") + "|" +
                    "|" +
                    "|" +
                     item.totalCOFINS.ToString().Replace(".", "") + "|" +
                     "|");
                contadorRegC495++;
            }

            conteudoRegC100.Append(conteudoRegC490);
            conteudoRegC490.Clear();
          
        }

        private void VerificarSuspensaoPisCofins(string filial,string codigo,string cstPIS,string cstCOFINS,decimal totalReceita)
        {
            try
            {
                if (cstPIS == "02" || cstPIS == "04" || cstPIS == "05" || cstPIS == "06" || cstPIS == "07" || cstPIS == "09")
                {
                    string naturezaRec = "000";
                    if (filial == "00001")
                    {
                        naturezaRec = (from n in Conexao.CriarEntidade().produtos
                                       where n.codigo == codigo
                                       select n.codigosuspensaopis).First();
                    }
                    if (filial != "00001")
                    {
                        naturezaRec = (from n in Conexao.CriarEntidade().produtosfilial
                                       where n.codigo == codigo
                                       && n.CodigoFilial == filial
                                       select n.codigosuspensaopis).First();
                    }

                    StruM410 novoM410 = new StruM410 { nat_rec = naturezaRec, vl_rec = totalReceita, cst = cstPIS };
                    listaM410.Add(novoM410);
                }

                if (cstCOFINS == "02" || cstCOFINS == "04" || cstCOFINS == "05" || cstCOFINS == "06" || cstCOFINS == "07" || cstCOFINS == "09")
                {
                    string naturezaRec = "000";
                    if (filial == "00001")
                    {
                        naturezaRec = (from n in Conexao.CriarEntidade().produtos
                                       where n.codigo == codigo
                                       select n.codigosuspensaocofins).First();
                    }
                    if (filial != "00001")
                    {
                        naturezaRec = (from n in Conexao.CriarEntidade().produtosfilial
                                       where n.codigo == codigo
                                       && n.CodigoFilial == filial
                                       select n.codigosuspensaocofins).First();
                    }

                    StruM410 novoM810 = new StruM410 { nat_rec = naturezaRec, vl_rec = totalReceita, cst = cstCOFINS };
                    listaM810.Add(novoM810);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Verificando suspensão PIS COFINS " + ex.Message);
            }
        }
        
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

                        var dadosItensAgr = from n in entidade.blococregc190
                                            where n.dataentrada >= dataInicial && n.dataentrada <= dataFinal
                                            && n.numero == itemNF.numero
                                            select n;
                        var dadosRegistrC190 = dadosItensAgr.ToList();
                       

                            if (!codigoParticipante.Contains(itemNF.codigofornecedor.ToString()))
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
                            itemNF.codigofornecedor + "F|" +
                            itemNF.modeloNF + "|" +
                            situacaoNF + "|" +
                            itemNF.serie + "|" +
                            itemNF.subserie + "|" +
                            itemNF.NF + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.DataEmissao) + "|" +
                            string.Format("{0:ddMMyyyy}", itemNF.dataEntrada) + "|" +
                            itemNF.ValorNota.ToString().Replace(".", "") + "|" +
                            itemNF.Icms.ToString().Replace(".", "") + "|" +
                            "|" +
                            itemNF.pis.ToString().Replace(".", "") + "|" +
                            itemNF.cofins.ToString().Replace(".", "") + "|"); // COD_GRUPO_TENSAO
                            contadorRegC500++;                        
                    }
                };
                #endregion Entrada

            };

            if (contadorRegC500 > 0)
            {
                registros.Add("C500|" + contadorRegC500.ToString());
                //registros.Add("C510|" + contadorRegC510.ToString());
                registros.Add("C501|" + contadorRegC501.ToString());
                registros.Add("C505|" + contadorRegC505.ToString());

            }

            #endregion Registro C100
        }


        private void BlocoCRegC100(string filial,bool gerarC170 = true)
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
                        var dadosEntradas = from n in entidade.moventradas
                                            where n.dataEntrada >= dataInicial.Date && n.dataEntrada <= dataFinal
                                            && n.modeloNF == item.modeloDocFiscal
                                            && n.Codigofilial == filial
                                            && n.exportarfiscal == "S"
                                            && n.lancada == "X"
                                            && n.NF !=""
                                            && (n.cfopentrada == "1.102" || n.cfopentrada == "2.102" || n.cfopentrada == "1.403" ||
                                            n.cfopentrada == "1.201" || n.cfopentrada == "1.202" || n.cfopentrada == "2.201" || n.cfopentrada == "2.202")
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
                                                n.NF
                                            };
                        var RegistroEntrada = dadosEntradas.ToList();

                        foreach (var itemNF in RegistroEntrada)
                        {

                            var dadosItens = from n in Conexao.CriarEntidade().registro50entradas_itens
                                             where n.dataentrada >= dataInicial.Date && n.dataentrada <= dataFinal.Date
                                             && n.numero == itemNF.numero
                                             && n.totalProduto > 0
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
                                                 n.ipi

                                             };
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

                            string dataEmissao = string.Format("{0:ddMMyyyy}", itemNF.DataEmissao);
                            string dataEntrada = string.Format("{0:ddMMyyyy}", itemNF.dataEntrada);
                            string indicadorPagamento = itemNF.indicadorpagamento;
                            decimal descontos = itemNF.descontos;
                            string tipoFrete = itemNF.tipofrete;
                            
                            if (string.IsNullOrEmpty(tipoFrete.Trim()))
                                tipoFrete = "9";
                            
                            decimal valorFrete = itemNF.Frete;
                            decimal valorSeguro = itemNF.valorseguro;
                            decimal despesas = itemNF.Despesas;
                            decimal pisST = itemNF.pis_st;
                            decimal cofinsST = itemNF.cofins_st;

                            // REgras para evitar erro de validacao
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
                            "|" + // Retirado por que apresentou inconsistenciaitemNF.chave_nfe + "|" +
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
                            "0|" + // Não está calculado o ST ainda itemNF.BaseIcmsSubst.ToString().Replace(".", "") + "|" +
                            "0|" + // Não está calculado o ST ainda itemNF.IcmsSubst.ToString().Replace(".", "") + "|" +
                            totalIPI.ToString().Replace(".", "") + "|" +
                            totalPIS.ToString().Replace(".", "") + "|" +
                            totalCOFINS.ToString().Replace(".", "") + "|" +
                            pisST.ToString().Replace(".", "") + "|" +
                            cofinsST.ToString().Replace(".", "") + "|"); // VL_COFINS_ST
                            contadorRegC100++;

                                int sequencia = 1;
                                foreach (var registroItem in itensNota)
                                {
                                    //if (!codigos.Contains(registroItem.codigo))
                                    //    codigos.Add(registroItem.codigo);

                                    string cod = (from n in codigos
                                                  where n.codigo == registroItem.codigo
                                                  //&& n.filial == filial
                                                  select n.codigo).FirstOrDefault();

                                    if (cod == null || string.IsNullOrEmpty(cod))
                                    {
                                        StruCodigos novoCod = new StruCodigos()
                                        {
                                            codigo = registroItem.codigo,
                                            filial = filial
                                        };
                                        codigos.Add(novoCod);
                                    }

                                    if (!unidades.Contains(registroItem.unidade))
                                        unidades.Add(registroItem.unidade);

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
                                        "0|" + // VL_BC_ICMS_ST
                                        "0|" + // ALIQ_ST
                                        "0|" + // VL_ICMS_ST
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
                    
                    if (gerarRegC100Saida)
                    {
                        var dadosSaida = from n in entidade.contnfsaida
                                         where n.dataemissao >= dataInicial.Date && n.dataemissao <= dataFinal.Date
                                         && n.modelodocfiscal == item.modeloDocFiscal
                                         && n.codigofilial == filial
                                         && n.finalidade == "1"
                                         && n.codcliente > 0
                                         && n.exportarfiscal == "S"
                                         && n.chave_nfe !=""
                                         && (n.cfop=="5.102" || n.cfop =="6.101" || n.cfop=="6.102" || n.cfop=="5.405")                                         
                                         select n;
                        var RegistroSaida = dadosSaida.ToList();

                        foreach (var itemNF in RegistroSaida)
                        {
                            string notaFiscal = Convert.ToString(itemNF.notafiscal.Value).Trim();

                            var dadosItens = from n in entidade.registro50saidas_itens
                                             where n.DATA >= dataInicial.Date && n.DATA <= dataFinal.Date
                                             && n.notafiscal == notaFiscal && n.serienf == itemNF.serie
                                             && n.codigofilial == filial
                                             select n;
                            var itensNota = dadosItens.ToList();

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



                            string tipoParticipante = "C";
                            if (itemNF.codcliente > 0)
                            {
                                if (!codigoParticipante.Contains("C" + itemNF.codcliente.ToString()))
                                    codigoParticipante.Add("C" + itemNF.codcliente.ToString());
                            }
                            if (itemNF.codfornecedor > 0)
                            {
                                if (!codigoParticipante.Contains("F" + itemNF.codfornecedor.ToString()))
                                    codigoParticipante.Add("F" + itemNF.codfornecedor.ToString());
                                tipoParticipante = "F";
                            }
                            
                           
                            string emitente = "0";                            
                            Int32 codDestinatario = itemNF.codcliente > 0 ? itemNF.codcliente : itemNF.codfornecedor;

                            //string situacaoNF = itemNF.situacaoNF == "N" ? "00" : "02";
                            string situacaoNF = itemNF.situacaoNF;
                            if (situacaoNF == "S")
                                situacaoNF = "02";
                            if (situacaoNF == "N")
                                situacaoNF = "00";
                            if (situacaoNF.Trim().Count() == 1)
                                situacaoNF = "00";

                            bool nfCancelada = false;
                            if (situacaoNF == "S" || situacaoNF == "02" || situacaoNF == "03" || situacaoNF == "04" && situacaoNF == "05")
                                nfCancelada = true;


                            if (!nfCancelada)
                            {
                                conteudoRegC100.AppendLine("|C100|" +
                                "1|" + //IND_OPER 0:Entrada 1:Saida
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
                                  "|" + // cod_emitente
                                  itemNF.modelodocfiscal + "|" +
                                  situacaoNF + "|" +
                                  itemNF.serie + "|" +
                                  itemNF.notafiscal + "|" +
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
                                 "|" +
                                 "|" + // VL_COFINS
                                 "|" +
                                 "|"); // VL_COFINS_ST
                            }


                            if (itemNF.idinfocomplementar > 0)
                            {
                                if (!codigoInfoCompNF.Contains(itemNF.idinfocomplementar.ToString()))
                                    codigoInfoCompNF.Add(itemNF.idinfocomplementar.ToString());

                                conteudoRegC100.AppendLine("|C110|" +
                                    itemNF.idinfocomplementar.ToString() + "|" +
                                    itemNF.obs.Trim().Replace("|","") + "|");
                                contadorRegC110++;
                            }



                            contadorRegC100++;

                            if (gerarC170 == true && !nfCancelada)
                            {

                                int sequencia = 1;
                                foreach (var registroItem in itensNota)
                                {
                                    //if (!codigos.Contains(registroItem.codigo))
                                    //    codigos.Add(registroItem.codigo);

                                    string cod = (from n in codigos
                                                  where n.codigo == registroItem.codigo
                                                //  && n.filial == filial
                                                  select n.codigo).FirstOrDefault();

                                    if (cod == null || string.IsNullOrEmpty(cod))
                                    {
                                        StruCodigos novoCod = new StruCodigos()
                                        {
                                            codigo = registroItem.codigo,
                                            filial = filial
                                        };
                                        codigos.Add(novoCod);
                                    }
                                    if (!unidades.Contains(registroItem.unidade))
                                        unidades.Add(registroItem.unidade);                                    

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

                                    VerificarSuspensaoPisCofins(filial, registroItem.codigo, registroItem.cstpis, registroItem.cstcofins, registroItem.SUM_TOTAL_.Value);
                                }

                            }


                        };
                    };

                } // If GerarSaida
                #endregion Saida
            };

            

            #endregion Registro C100
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


            int totalRegistro = totalReg0 + totalRegA + totalRegC + totalRegD + totalRegF + totalRegM + totalReg1 + totalReg9;

            conteudoReg9001.AppendLine("|9999|" +
                    Convert.ToString(totalRegistro) + "|");
            #endregion BLOCO 9
        }


        private void BlocoA(string acao)
        {
            if (acao == "abertura")
            {
               conteudoBlocoA.AppendLine("|A001|" +
                    "1|");
                totalRegA++;
                registros.Add("A001|1");
            }
            if (acao == "fechamento")
            {

                conteudoBlocoA.AppendLine("|A990|" +
                    "2|");
                totalRegA++;
                registros.Add("A990|1");
            }

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


        private void BlocoF(string acao)
        {
            if (acao == "abertura")
            {
                conteudoBlocoF.AppendLine("|F001|" +
                     "1|");
                totalRegF++;
                registros.Add("F001|1");
            }
            if (acao == "fechamento")
            {

                conteudoBlocoF.AppendLine("|F990|" +
                    "2|");
                totalRegA++;
                registros.Add("F990|1");
            }

        }

        private void BlocoM(string acao)
        {
            if (acao == "abertura")
            {
                string movimento = "1";

                if (listaM410.Count()>0 || listaM810.Count()>0)
                {
                    movimento = "0";
                }
                conteudoBlocoM.AppendLine("|M001|" +
                    movimento+"|");
                totalRegM++;
                registros.Add("M001|1");
            }

            if (acao == "fechamento")
            {
                totalRegM += contadorRegM410 + contadorRegM810;

                if (contadorRegM410 > 0)
                    registros.Add("M410|" + contadorRegM410.ToString());

                if (contadorRegM810 > 0)
                    registros.Add("M810|" + contadorRegM810.ToString());

                totalRegM++;
                conteudoBlocoM.AppendLine("|M990|" + totalRegM.ToString() + "|");                
                registros.Add("M990|1");
                          
            }

        }

        private void BlocoMRegM410()
        {
            try
            {

                var dadosM400 = from p in listaM410
                                group p by new { cst = p.cst } into g
                                select new { g.Key.cst, valor = g.Sum(p => p.vl_rec) };

                foreach (var item in dadosM400)
                {
                    conteudoBlocoM.AppendLine("|M400|" +
                       item.cst + "|" +
                       item.valor.ToString().Replace(".", "") + "|" +
                       "|" +
                       "|");
                    registros.Add("M400|1");
                    totalRegM++;
                }


                var dadosM410 = from p in listaM410
                                group p by new { natureza = p.nat_rec } into g
                                select new { g.Key.natureza, valor = g.Sum(p => p.vl_rec) };
                foreach (var item in dadosM410)
                {
                    conteudoBlocoM.AppendLine("|M410|" +
                        item.natureza + "|" +
                        item.valor.ToString().Replace(".", "") + "|" +
                        "|" +
                        "|");
                    contadorRegM410++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("M410 - " + ex.Message);
            }

        }

        private void BlocoMRegM810()
        {
            try
            {
                var dadosM800 = from p in listaM810
                                group p by new { cst = p.cst } into g
                                select new { g.Key.cst, valor = g.Sum(p => p.vl_rec) };

                foreach (var item in dadosM800)
                {
                    conteudoBlocoM.AppendLine("|M800|" +
                       item.cst + "|" +
                       item.valor.ToString().Replace(".", "") + "|" +
                       "|" +
                       "|");
                    registros.Add("M800|1");
                    totalRegM++;
                }

                var dadosM410 = from p in listaM810
                                group p by new { natureza = p.nat_rec } into g
                                select new { g.Key.natureza, valor = g.Sum(p => p.vl_rec) };
                foreach (var item in dadosM410)
                {
                    conteudoBlocoM.AppendLine("|M810|" +
                        item.natureza + "|" +
                        item.valor.ToString().Replace(".", "") + "|" +
                        "|" +
                        "|");
                    contadorRegM810++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("M810 - " + ex.Message); 
            }
        }


        private void Bloco0()
        {
            #region BLOCO 0

            #region BLOCO 0 Registro 0000
            conteudoReg0000.AppendLine("|0000|" + // 01 - REG
                codigoLayout + "|" + // 02 cod_ver
                codifoFinalidade + "|" + //03 Tipo_Escrituração
                "0|" + // 04 Ind_Sit_Esp 0 = Abertura 1 = Cisao 2 = Fusao ...
                new string(' ', 41) + "|" +
                string.Format("{0:ddMMyyy}", dataInicial) + "|" +
                string.Format("{0:ddMMyyy}", dataFinal) + "|" +
                nomeEmpresa + "|" +
                cnpj + "|" +
                uf + "|" +
                cod_mun + "|" +
                suframa + "|" +
                "00|" + //13 Ind_NAT_PJ 00 = Sociedade empresaria geral                
                "2|"); // 2 - Atividade de Comércio ind_ativ + "|");
            totalReg0++;
            registros.Add("0000|1");
            #endregion Bloco 0 Registro 0000
            #region BLOCO 0 Registro 0001
            conteudoReg0001.AppendLine("|0001|" +
                 "0|");
            contadorReg0001++;
            registros.Add("0001|" + contadorReg0001.ToString());
            #endregion Fim BLOCO 0 Registro 0001            
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
            #region Bloco 0 Registro 0110 - Regimento apuração
            conteudoReg0110.AppendLine("|0110|" +
                cod_inc_trib + "|" +
                ind_apro_cred + "|" +
                cod_tipo_cont + "|");
               // ind_reg_cum + "|");
            contadorReg0110++;
            registros.Add("0110|" + contadorReg0110.ToString());

            #endregion 0110

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
                    }


                    conteudoReg0150.AppendLine("|0150|" +
                    codigo + item.Substring(0, 1) + "|" +
                    nome.Trim() + "|" +
                    "01058|" +
                    cnpjCli + "|" +
                    cpfCli + "|" +
                    ieCli.Trim().Replace(".", "").Replace("-", "").Replace("ISENTO", "") + "|" +
                    Funcoes.RetornaCodigoMunIBGE("M", cidadeCli, estadoCli) + "|" +
                    "|" + //SUFRAMA
                    enderecoCli.Replace("|", "") + "|" +
                    numeroCli + "|" +
                    complementoCli + "|" +
                    bairroCli + "|");
                    contadorReg0150++;
                }                 
                catch (Exception ex)
                {
                    MessageBox.Show("F-ornecedor ou C-liente nao encontrado " + item.ToString() +" Ex: "+ex.Message);
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
                    dadosItem.ProcurarCodigo(registro.codigo, registro.filial, false);
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
                            tabela.Value = registro.filial;

                            EntityParameter parFilial = cmd.Parameters.Add("novoCodigo", DbType.String);
                            parFilial.Direction = ParameterDirection.Input;
                            parFilial.Value = registro.codigo;
                         
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            dadosItem.ProcurarCodigo(registro.codigo, registro.filial, false);
                        }                       
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Exceção ao processar SP recriarCodigoProduto " + ex.Message);
                    }

                }
               

                if (!conteudoReg0200.ToString().Contains("|"+dadosItem.codigo+"|"+dadosItem.descricao+"|")) // string.IsNullOrEmpty(adicionar))
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
                           ncm + "|" +
                           nbm + "|" + // TIPI
                           ncmespecie + "|" + // GEN
                           codigoServico + "|" +
                           dadosItem.icms + "|");
                    contadorReg0200++;
                    if (!unidades.Contains(dadosItem.unidade))
                        unidades.Add(dadosItem.unidade);
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

            #region Bloco 0 Registro 0450: Tabela de Informação Complementar Documento Fiscal

            var codigoInfComUnico = (from n in codigoInfoCompNF
                                     select n).Distinct();

            foreach (var item in codigoInfComUnico)
            {
                int id = Convert.ToInt16(item);
                var reg0450 = (from n in Conexao.CriarEntidade().infocomplementarnf
                              where n.id == id
                              select new { n.id, n.descricao }).First();
                conteudoReg0450.AppendLine("|0450|" +
                    reg0450.id.ToString() + "|" +
                    reg0450.descricao + "|");
                contadorReg0450++;                
            }
            registros.Add("0450|" + contadorReg0450.ToString());
           

            #endregion


            #region BLOCO 0 Registro 0990: Encerramento do Bloco 0
            totalReg0 += (contadorReg0001 + contadorReg0140 + contadorReg0150 + contadorReg0100 + contadorReg0110 +
                contadorReg0190 + contadorReg0200 + contadorReg0450 ) + 1;
            conteudoReg0990.AppendLine("|0990|" +
                totalReg0.ToString() + "|");
            registros.Add("0990|1");
            #endregion
            #endregion BLOCO 0;

            // Aqui depois que verificou os estabelecimentos nos registro C100
            registros.Add("0140|" + contadorReg0140.ToString());
        }

    }

    public struct StruM410
    {
        public string nat_rec;
        public string cst;
        public decimal vl_rec;
    }

    public struct StruM810
    {
        public string nat_rec;
        public string cst;
        public decimal vl_rec;

    }
   
}

    