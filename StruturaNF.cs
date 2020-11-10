using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SICEpdv
{
    class Destinatario
    {
        public string CPF { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string bairro { get; set; }
        public int codigoMunicipio { get; set; }
        public string municipio { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }
        public string Email { get; set; }
    }

    class ProdutoSimples
    {
        public string Aliquota { get; set; } //T+aliquota ou S+aliquota S=Serviço ISSQN T=Tributacao ICMS
        public decimal quantidade { get; set; }
        public decimal precoUnitario { get; set; }
        public TipoDescAcrescItem TipoDescAcresc { get; set; }//$D ou $A
        public decimal ValorDescAcresc { get; set; }
        public string codigoitem { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }

    }

    class ProdutoCompleto
    {
        public string Aliquota { get; set; } //T+aliquota ou S+aliquota S=Serviço ISSQN T=Tributacao ICMS
        public decimal quantidade { get; set; }
        public decimal precoUnitario { get; set; }
        public string TipoDescAcresc { get; set; }//$D ou $A
        public decimal ValorDescAcresc { get; set; }
        public string codigoitem { get; set; }
        public string unidade { get; set; }
        public string descricao { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
    }

    class CancelarNF
    {
        public string numero { get; set; }
        public string serie { get; set; }
        public string chaveAcesso { get; set; }
        public string protocoloAutorizacao { get; set; }
        public string justificativa { get; set; } // no minomo 15
    }

    class Inutilizacao
    {
        public string NumeroInicial { get; set; }
        public string NumeroFinal { get; set; }
        public string NumeroSerie { get; set; }
        public string Justificativa { get; set; }
    }

    class retornoInformacao
    {
        public TipoItervalo TipoIntervalor { get; set; }
        public string IndiceIncio { get; set; }
        public string IndiceFinal { get; set; }
        public string Serie { get; set; }
        public string ChaveAcesso { get; set; }
        public InformacaoRetorno InformecaoRetorno { get; set; } 
    }

    public enum TipoDescAcrescItem
    {
        [XmlEnum("D$")] Desconto,
        [XmlEnum("A$")] Acrescimo,
    }

    public enum TipoItervalo
    {
        [XmlEnum("DATA")] DATA,
        [XmlEnum("CHAVE")] CHAVE,
        [XmlEnum("NUM")] NUM
    }

    
    public enum TipoDescAcrescTotalizador
    {
        [XmlEnum("D$")] ValorDesconto,
        [XmlEnum("D%")] PercentualDesconto,
        [XmlEnum("A$")] ValorAcrescimo,
        [XmlEnum("D$")] PercentualAcrecimo
    }

    public enum FormasPagamento
    {
        [XmlEnum("01- Dinheiro")] Dinheiro,
        [XmlEnum("02- Cheque")] Cheque,
        [XmlEnum("03- Cartão de Crédito")] CartaoCredito,
        [XmlEnum("04- Cartão de Débito")] CartaoDebito,
        [XmlEnum("05- Crédito Loja")] CreditoLoja,
        [XmlEnum("10- Vale Alimentação")] ValeAlimentacao,
        [XmlEnum("11- Vale Refeição")] ValeRefeicao,
        [XmlEnum("12- Vale Presente")] ValePresente,
        [XmlEnum("13- Vale Combustível")] ValeCombustivel,
        [XmlEnum("99- Outros")] Outros,
    }

    public enum InformacaoRetorno
    {
        [XmlEnum("1")]
        DescricaoSituacaoNota = 1,
        [XmlEnum("2")]
        CodigoSituacaoNota = 2,
        [XmlEnum("3")]
        LinkXMLReferenteNota = 3,
        [XmlEnum("4")]
        DataEmissao = 4,
        [XmlEnum("5")]
        CNPJCPFDestinatarioInformado = 5,
        [XmlEnum("6")]
        NomeDestinatarioInformado = 6,
        [XmlEnum("7")]
        ValorTotalNota = 7,
        [XmlEnum("8")]
        ModeloNota = 8,
        [XmlEnum("9")]
        NumeroNota = 9,
        [XmlEnum("10")]
        SerieNota = 10,
        [XmlEnum("11")]
        ChaveAcessoNota = 11,
        [XmlEnum("13")]
        TodasInformacoesAnteriores = 13,
        [XmlEnum("14")]
        XMLVendaArquivoPDF = 14,
        [XmlEnum("15")]
        DANFEArquivoPDF = 15,
        [XmlEnum("16")]
        ProtocoloNota = 16
    }

    
}
