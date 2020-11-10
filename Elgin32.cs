using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SICEpdv
{
    class Elgin32
    {
        public static int Int_Retorno;

        #region DECLARAÇÕES DE MÉTODOS DE ACESSO A IMPRESSORA

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AberturaDoDia(string ValorCompra, string FormaPagamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreComprovanteNaoFiscalVinculado(string FormaPagamento, string Valor, string NumeroCupom);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreComprovanteNaoFiscalVinculadoMFD(string FormaPagamento,string Valor, string NumeroCupom, string CGC, string Nome, string Endereco);

        [DllImport("Elgin.dll")] 
        public static extern int Elgin_AbreCupom(string CGC_CPF);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreCupomMFD(string CGC,string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbrePortaSerial();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreRecebimentoNaoFiscalMFD(string CGC, string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreRelatorioGerencial();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AbreRelatorioGerencialMFD(string Indice);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AcionaGaveta();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AcionaGuilhotinaMFD(int TipoCorte);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AcrescimoDescontoItemMFD(string Item, string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AcrescimoDescontoSubtotalMFD(string cFlag, string cTipo, string cValor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AcrescimoDescontoSubtotalRecebimentoMFD(string cFlag, string cTipo, string cValor);

        [DllImport("Elgin.dll")] 
        public static extern int Elgin_AcrescimoItemNaoFiscalMFD(string strNroItem, string strAcrescDesc, string strTipoAcrescDesc, string strValor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Acrescimos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorAcrescimos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AlteraSimboloMoeda(string SimboloMoeda);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AtivaDesativaVendaUmaLinhaMFD(int iFlag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AumentaDescricaoItem(string Descricao);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Autenticacao();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_AutenticacaoMFD(string Linhas, string Texto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaAcrescimoDescontoItemMFD(string cFlag, string cItem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaAcrescimoDescontoSubtotalMFD(string cFlag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaAcrescimoDescontoSubtotalRecebimentoMFD(string cFlag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaAcrescimoNaoFiscalMFD(string strNumeroItem, string strAcrecDesc);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaCupom();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaCupomMFD(string CGC,string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaImpressaoCheque();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaItemAnterior();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaItemGenerico(string NumeroItem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaItemNaoFiscalMFD(string strNroItem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaLeituraBinario();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Cancelamentos(string ValorCancelamentos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CancelaRecebimentoNaoFiscalMFD(string CGC, string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CGC_IE([MarshalAs(UnmanagedType.VBByRefStr)] ref string CGC,[MarshalAs(UnmanagedType.VBByRefStr)] ref string IE);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ClicheProprietario([MarshalAs(UnmanagedType.VBByRefStr)] ref string Cliche);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CNPJ_IE([MarshalAs(UnmanagedType.VBByRefStr)] ref string CNPJ,[MarshalAs(UnmanagedType.VBByRefStr)] ref string IE);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CNPJMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string CNPJ);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasCODABARMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasCODE128MFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasCODE39MFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasCODE93MFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasEAN13MFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasEAN8MFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasISBNMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasITFMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasMSIMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasPLESSEYMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasUPCAMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CodigoBarrasUPCEMFD(string Codigo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ComprovantesNaoFiscaisNaoEmitidosMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Comprovantes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ConfiguraCodigoBarrasMFD(int Altura, int Largura, int pos, int Fonte, int Margem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadorComprovantesCreditoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Comprovantes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadorCupomFiscalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string CuponsEmitidos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadoresTotalizadoresNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Contadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadoresTotalizadoresNaoFiscaisMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Contadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadorFitaDetalheMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string ContadorFita);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadorOperacoesNaoFiscaisCanceladasMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string OperacoesCanceladas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ContadorRelatoriosGerenciaisMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Relatorios);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ConverteATO17ParaPAFRJ(string arquivoATO17);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_CupomAdicionalMFD();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DadosSintegra(string DataInicial, string DataFinal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DadosUltimaReducao([MarshalAs(UnmanagedType.VBByRefStr)] ref string DadosReducao);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DadosUltimaReducaoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string DadosReducao);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataHoraImpressora([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Hora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataHoraReducao([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Hora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataHoraSoftwareBasico([MarshalAs(UnmanagedType.VBByRefStr)] ref string DataSW, [MarshalAs(UnmanagedType.VBByRefStr)] ref string HoraSW);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataHoraUltimoDocumentoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cDataHora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataMovimento([MarshalAs(UnmanagedType.VBByRefStr)] ref string Data);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DataMovimentoUltimaReducaoMFD(string cDataMovimento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Descontos([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorDescontos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DownloadMF(string Arquivo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_DownloadMFD(string Arquivo, string TipoDownload, string ParametroInicial, string ParametroFinal, string UsuarioECF);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EfetuaFormaPagamento(string FormaPagamento, string ValorFormaPagamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EfetuaFormaPagamentoDescricaoForma(string FormaPagamento, string ValorFormaPagamento, string DescricaoFormaPagto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EfetuaFormaPagamentoMFD(string FormaPagamento, string ValorFormaPagamento, string Parcelas, string DescricaoFormaPagto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EfetuaRecebimentoNaoFiscalMFD(string IndiceTotalizador, string ValorRecebimento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EspacoEntreLinhas(int Dots);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EstornoFormasPagamento(string FormaOrigem, string FormaDestino, string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_EstornoNaoFiscalVinculadoMFD(string CGC, string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ExecutaComando(string Comando, string parametros);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ExecutaLeitura(string Comando, string parametros, [MarshalAs(UnmanagedType.VBByRefStr)] ref string retorno);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaComprovanteNaoFiscalVinculado();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaCupom(string FormaPagamento, string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto, string ValorPago, string Mensagem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaCupomResumido(string FormaPagamento, string Mensagem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechamentoDoDia();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaPortaSerial();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaRecebimentoNaoFiscalMFD(string Mensagem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FechaRelatorioGerencial();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FlagsFiscais(ref int Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FlagsFiscaisStr([MarshalAs(UnmanagedType.VBByRefStr)] ref string FlagFiscal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_FormatoDadosMFD(string ArquivoOrigem, string ArquivoDestino, string TipoFormato, string TipoDownload, string ParametroInicial, string ParametroFinal, string UsuarioECF);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_GeraArquivoATO17Binario(string szArquivoBinario, string szArquivoTexto, string szPeriodoIni, string szPeriodoFIM, Byte tipoPeriodo, string szUsuario, string szTipoLeitura);
        
        [DllImport("Elgin.dll")]
        public static extern int Elgin_GeraRFDBinario(string periodoInicial, string periodoFinal, int tipoPeriodo, int tipoLeitura, [MarshalAs(UnmanagedType.VBByRefStr)] ref string nomeArquivo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_GeraRFDBinarioRJ(string periodoInicial, string periodoFinal, int tipoPeriodo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_GrandeTotal([MarshalAs(UnmanagedType.VBByRefStr)] ref string GrandeTotal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_GrandeTotalUltimaReducaoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cGT);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_HabilitaDesabilitaRetornoEstendidoMFD(string FlagRetorno);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_IdentificaConsumidor(string CNPJ_CPF, string Nome, string Endereco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ImprimeCheque(string Banco, string Valor, string Favorecido, string Cidade, string Data, string Mensagem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ImprimeChequeMFD(string NumeroBanco, string Valor, string Favorecido, string Cidade, string Data, string Mensagem, string ImpressaoVerso, string Linhas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ImprimeConfiguracoesImpressora();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ImprimeCopiaCheque();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ImprimeDepartamentos();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_IncluiCidadeFavorecido(string Cidade, string Favorecido);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_IniciaFechamentoCupom(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimoDesconto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_IniciaFechamentoCupomMFD(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimo, string ValorDesconto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_IniciaFechamentoRecebimentoNaoFiscalMFD(string AcrescimoDesconto, string TipoAcrescimoDesconto, string ValorAcrescimo, string ValorDesconto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_InicioFimCOOsMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cCOOIni, [MarshalAs(UnmanagedType.VBByRefStr)] ref string cCOOFim);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_InicioFimGTsMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cGTIni, [MarshalAs(UnmanagedType.VBByRefStr)] ref string cGTFim);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_InscricaoEstadualMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string InscricaoEstadual);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_InscricaoMunicipalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string InscricaoMunicipal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeArquivoRetorno([MarshalAs(UnmanagedType.VBByRefStr)] ref string sCupom);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeCodigoNacionalIdentificacaoECF([MarshalAs(UnmanagedType.VBByRefStr)] ref string CNI);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeIndicadores(ref int indicador);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraCheque([MarshalAs(UnmanagedType.VBByRefStr)] ref string CodigoCMC7);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraMemoriaFiscalData(string DataInicial, string DataFinal, string FlagLeitura);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraMemoriaFiscalReducao(string ReducaoInicial, string ReducaoFinal, string FlagLeitura);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraMemoriaFiscalSerialData(string DataInicial, string DataFinal, string FlagLeitura);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraMemoriaFiscalSerialReducao(string ReducaoInicial, string ReducaoFinal, string FlagLeitura);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraX();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeituraXSerial();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeMemoriasBinario(string szNomeArquivo, string szSerieECF, bool bAguardaConcluirLeitura);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeNomeRelatorioGerencial(string Codigo, string NomeRelatorio);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeParametrosPAF(string CNPJ, string Data, string Hora, string NumSerie, string NumECF, string GrandeTotal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LeStatusGeraBinario(ref int nSituacaoAtual, ref int nCodigoErro, ref int nTamanhoLeitura, ref int nProgressoLeitura, [MarshalAs(UnmanagedType.VBByRefStr)] ref string strSituacaoAtual);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_LinhasEntreCupons(int Linhas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MapaResumo();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MapaResumoMFD();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MarcaModeloTipoImpressoraMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Marca, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Modelo, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Tipo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MinutosEmitindoDocumentosFiscaisMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Minutos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MinutosImprimindo([MarshalAs(UnmanagedType.VBByRefStr)] ref string Minutos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_MinutosLigada([MarshalAs(UnmanagedType.VBByRefStr)] ref string Minutos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ModeloImpressora([MarshalAs(UnmanagedType.VBByRefStr)] ref string ModeloImpressora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NomeiaDepartamento(int Indice, string Departamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NomeiaRelatorioGerencialMFD(string Indice, string Descricao);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NomeiaTotalizadorNaoSujeitoIcms(int Indice, string Totalizador);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroCaixa([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCaixa);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroCupom([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCupom);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroCuponsCancelados([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroCancelamentos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroIntervencoes([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroIntervencoes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroLoja([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroLoja);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroOperacoesNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroOperacoes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroReducoes([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroReducoes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroSerie([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerie);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroSerieCriptografado([MarshalAs(UnmanagedType.VBByRefStr)]  ref string NumeroSerie);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroSerieDescriptografado(string NumeroSerieCriptografado,[MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerieDesCriptografado);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroSerieMemoriaMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSerieMFD);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_NumeroSubstituicoesProprietario([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroSubstituicoes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_PercentualLivreMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cMemoriaLivre);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaAliquota(string Aliquota, int ICMS_ISS);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaArredondamento();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaBaudRate(string BaudRate);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaCaracterAutenticacao(string parametros);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaFormaPagamentoMFD(string FormaPagto, string OperacaoTef);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaHorarioVerao();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaIdAplicativoMFD(string NomeAplicativo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaMoedaPlural(string MoedaPlural);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaMoedaSingular(string MoedaSingular);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaOperador(string NomeOperador);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ProgramaTruncamento();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RecebimentoNaoFiscal(string IndiceTotalizador, string Valor, string FormaPagamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ReducaoZ(string Data, string Hora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ReducoesRestantesMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Reducoes);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RegistrosTipo60();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ReimpressaoNaoFiscalVinculadoMFD();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RelatorioGerencial(string Texto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RelatorioSintegraMFD(int iRelatorios, string cArquivo, string cMes, string cAno, string cRazaoSocial, string cEndereco, string cNumero, string cComplemento,string cBairro, string cCidade, string cCEP, string cTelefone, string cFax, string cContato);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RelatorioTipo60Analitico();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RelatorioTipo60AnaliticoMFD();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RelatorioTipo60Mestre();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ResetaImpressora();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RetornoAliquotas([MarshalAs(UnmanagedType.VBByRefStr)] ref string Aliquotas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_RetornoImpressora(ref int i,[MarshalAs(UnmanagedType.VBByRefStr)] ref string ErrorMsg);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Sangria(string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_SegundaViaNaoFiscalVinculadoMFD();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_SimboloMoeda([MarshalAs(UnmanagedType.VBByRefStr)] ref string SimboloMoeda);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_StatusEstendidoMFD(ref int iStatus);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_SubTotal([MarshalAs(UnmanagedType.VBByRefStr)] ref string SubTotal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_SubTotalComprovanteNaoFiscalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cSubTotal);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_Suprimento(string Valor, string FormaPagamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TamanhoTotalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cTamanhoMFD);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TempoOperacionalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string TempoOperacional);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TerminaFechamentoCupom(string Mensagem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TerminaFechamentoCupomCodigoBarrasMFD(string cMensagem, string cTipoCodigo, string cCodigo, int iAltura, int iLargura, int iPosicaoCaracteres, int iFonte, int iMargem, int iCorrecaoErros, int iColunas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TotalDiaTroco([MarshalAs(UnmanagedType.VBByRefStr)] ref string TotalDiaTroco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TotalDocTroco([MarshalAs(UnmanagedType.VBByRefStr)] ref string TotalDocTroco);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TotalIcmsCupom([MarshalAs(UnmanagedType.VBByRefStr)] ref string TotalICMS);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_TotalLivreMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string cMemoriaLivre);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_UltimoItemVendido([MarshalAs(UnmanagedType.VBByRefStr)] ref string NumeroItem);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_UsaComprovanteNaoFiscalVinculado(string Texto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_UsaRelatorioGerencialMFD(string Texto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ValorFormaPagamento([MarshalAs(UnmanagedType.VBByRefStr)] ref string FormaPagamento, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ValorFormaPagamentoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string FormaPagamento, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ValorPagoUltimoCupom([MarshalAs(UnmanagedType.VBByRefStr)] ref string ValorCupom);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ValorTotalizadorNaoFiscal([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizador, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_ValorTotalizadorNaoFiscalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizador, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Valor);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VendaBruta([MarshalAs(UnmanagedType.VBByRefStr)] ref string VendaBruta);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VendaLiquida([MarshalAs(UnmanagedType.VBByRefStr)] ref string VendaLiquida);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VendeItem(string Codigo,string Descricao, string Aliquota, string TipoQuantidade, string Quantidade, int CasasDecimais, string ValorUnitario, string TipoDesconto, string Desconto);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VendeItemDepartamento(string Codigo, string Descricao, string Aliquota, string ValorUnitario, string Quantidade, string Acrescimo, string Desconto, string IndiceDepartamento, string UnidadeMedida);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaAliquotasICMS([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaAliquotasIss([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaDepartamentos([MarshalAs(UnmanagedType.VBByRefStr)] ref string Departamentos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaEstadoGaveta(ref int EstadoGaveta);

        [DllImport("Elgin.dll")]
        public static extern int  Elgin_VerificaEstadoGavetaStr([MarshalAs(UnmanagedType.VBByRefStr)] ref string EstadoGaveta);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaEstadoImpressora(ref int ACK, ref int ST1, ref int ST2);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaEstadoImpressoraMFD(ref int ACK, ref int ST1, ref int ST2, ref int ST3);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaEstadoImpressoraStr(string ACK, string ST1, string ST2);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaFormasPagamento([MarshalAs(UnmanagedType.VBByRefStr)] ref string Formas);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaFormasPagamentoMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string FormasPagamento);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaImpressoraLigada();

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaIndiceAliquotasICMS([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaIndiceAliquotasIss([MarshalAs(UnmanagedType.VBByRefStr)] ref string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaModoOperacao([MarshalAs(UnmanagedType.VBByRefStr)] ref string Modo);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaRecebimentoNaoFiscal([MarshalAs(UnmanagedType.VBByRefStr)] ref string Recebimentos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaRecebimentoNaoFiscalMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Recebimentos);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaReducaoZAutomatica(ref int Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaRelatorioGerencialMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Relatorios);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaSensorPoucoPapelMFD(string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaStatusCheque(ref int StatusCheque);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTipoImpressora(ref int TipoImpressora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTipoImpressoraStr([MarshalAs(UnmanagedType.VBByRefStr)] ref string TipoImpressora);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTotalizadoresNaoFiscais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTotalizadoresNaoFiscaisMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTotalizadoresParciais([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTotalizadoresParciaisMFD([MarshalAs(UnmanagedType.VBByRefStr)] ref string Totalizadores);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaTruncamento(string Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VerificaZPendente(ref int Flag);

        [DllImport("Elgin.dll")]
        public static extern int Elgin_VersaoFirmware([MarshalAs(UnmanagedType.VBByRefStr)] ref string VersaoFirmware);

        [DllImport("Elgin.dll")]
        public static extern int RFD_ConvertedaMFD(string CRZ);

        [DllImport("Elgin.dll")]
        public static extern int RFD_ConvertedaMFDData(string DataInicial, string DataFinal);


        #endregion
    }
}
