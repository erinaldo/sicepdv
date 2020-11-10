using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    public class CaixaFiscal
    {
        public decimal baseICMS { get; set; }
        public decimal imposto { get; set; }
        public string tributacao { get; set; }
        public decimal icms { get; set; }
        public decimal valorProdutos { get; set; }
    }

    public class clientesModel
    {
        public string CodigoFilial { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string apelido { get; set; }
        public string ativo { get; set; }
        public string sexo { get; set; }
        public string observacao { get; set; }
        public string situacao { get; set; }
        public string cidade { get; set; }
        public string endereco { get; set; }
        public decimal debito { get; set; }
        public decimal debitoch { get; set; }
        public decimal credito { get; set; }
        public decimal saldo { get; set; }
        public decimal saldoch { get; set; }
    }

    public class dadosNFCe
    {
        public string chaveNFe { get; set; }
        public string protocolo { get; set; }
        public DateTime dataAutorizacao { get; set; }
        public string horaAutorizacao { get; set; }
        public string serieNF { get; set; }
        public string numeroNF { get; set; }
        public string modelo { get; set; }
        public string acao { get; set; }
        public string codigoProduto { get; set; }
        public decimal total { get; set; }
        public decimal baseICMS { get; set; }
        public decimal valorIcms { get; set; }
        public DateTime dataVenda { get; set; }
        public decimal totaldocumento { get; set; }
        public string grupo { get; set; }
        public decimal cfopOut { get; set; }
        public decimal cfopST { get; set; }
        public bool cancelado { get; set; }
        
    }

    public class dadosDAV
    {
        public string statusPagamento { get; set; }
        public string formapagamento { get; set; }
        public string origem { get; set; }
        public string vendaatacado { get; set; }
        public string idTransacaogateway { get; set; }
        public string status { get; set; }
    }

    public class erroNFCe
    {
        public string erro { get; set; }
    }

    public class XMLNFCe
    {
        public string XML { get; set; }
        public string chaveAcesso { get; set; }
        public int notafiscal { get; set; }
        public int serie { get; set; }
        public int documento { get; set; }
    }

    public class produtoLote
    {
        public string lote { get; set; }
        public decimal quantidade { get; set; }
        public DateTime vencimento { get; set; }
        public DateTime fabricacao { get; set; }
        public int codigoFornecedor { get; set; }
        public int inc { get; set; }
        public string codigoProduto { get; set; }
        public decimal quantidadeDigitada { get; set; }
    }

    public class documentosXML
    {
        public int Documento { get; set; }
        public string Estornado { get; set; }
        public decimal VlrTotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public DateTime Data { get; set; }
        public string Numero { get; set; }
        public string Serie { get; set; }
        public string Chave { get; set; }
        public string Protocolo { get; set; }
        public string Modelo { get; set; }
        public string Vendedor { get; set; }
        public string NFe { get; set; }
        public string Operador { get; set; }
        public string Cliente { get; set; }
    }

    public class transfvenda
    {
        public string ip { get; set; }
        public DateTime DATA { get; set; }
        public string operador { get; set; }
        public string codigo { get; set; }
        public string descricao { get; set; }
        public Decimal quantidade { get; set; }
        public Decimal preco { get; set; }
        public Decimal custo { get; set; }
        public string filial { get; set; }
        public string filialorigem { get; set; }
        public string numeroDAV { get; set; }
        public string documento { get; set; }
        public string transferencia { get; set; }
        public string cancelado { get; set; }
        public string quantidadeatualizada { get; set; }
        public string id { get; set; }
        public string quantidaestoqueorigem { get; set; }

    }

    public class sugestaoProdutos
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string quantidadeImpulso { get; set; }
        public string validadeImpulso { get; set; }
        public string codigoFilial { get; set; }
        public string precovenda { get; set; }
        public string relevancia { get; set; }
    }

    public class itensDocumento
    {
        public string codigo { get; set; }
        public string produto { get; set; }
        public string codigobarras { get; set; }
        public string tributacao { get; set; }
        public string icms { get; set; }
        public string cfop { get; set; }
        public string cest { get; set; }
        public string cstpis { get; set; }
        public string pis { get; set; }
        public string cstcofins { get; set; }
        public string cofins { get; set; }
    }

    public class nfe012E
    {
        public nfe012E notafiscal(string notafiscal, string serie)
        {
            string SQL = "SELECT IFNULL(CbdEmpCodigo,0) AS CbdEmpCodigo,IFNULL(CbdNtfNumero,0) AS CbdNtfNumero,IFNULL(CbdNtfSerie,0) AS CbdNtfSerie,IFNULL(CbdAcao,'') AS CbdAcao,IFNULL(CbdSituacao,'') AS CbdSituacao, " +
                   "IFNULL(CbdDtaProcessamento, 0) AS CbdDtaProcessamento, IFNULL(CbdNumProtocolo,'') AS CbdNumProtocolo, IFNULL(CbdStsRetCodigo,'') AS CbdStsRetCodigo, IFNULL(CbdStsRetNome,'') AS CbdStsRetNome," +
                   "IFNULL(CbdProcStatus,'') AS CbdProcStatus, IFNULL(CbdNFEChaAcesso,'') AS CbdNFEChaAcesso, IFNULL(CbdxMotivo,'') AS CbdxMotivo, IFNULL(CbdXML,'') AS CbdXML," +
                   "IFNULL(CbdNumProtCanc,'') AS CbdNumProtCanc, IFNULL(CbdDtaCancelamento,'') AS CbdDtaCancelamento, IFNULL(CbdDtaInutilizacao,'') AS CbdDtaInutilizacao, IFNULL(CbdNumProtInut,'') AS CbdNumProtInut, " +
                   "IFNULL(CbdMarca,'') AS CbdMarca, IFNULL(CbdDigVal,'') AS CbdDigVal, IFNULL(cbdcodigofilial,'') AS cbdcodigofilial, IFNULL(cbdCCe,'') AS cbdCCe, IFNULL(cbdCCePath,'') AS cbdCCePath," +
                   "IFNULL(cbdCCenSeqEvento,'') AS cbdCCenSeqEvento, IFNULL(CbdMod,'') AS CbdMod, IFNULL(cbdDocumento,'') AS cbdDocumento, IFNULL(cbdNRec,'') AS cbdNRec," +
                   "IFNULL(cbdVerificado,'') AS cbdVerificado, IFNULL(CdbNtfNumeroFinal,'') AS CdbNtfNumeroFinal, IFNULL(Cbdflag,'') AS Cbdflag, IFNULL(count(1),0) as qtdNota, IFNULL(DATEDIFF(CURRENT_DATE, DATE(CbdDtaProcessamento)),0) AS dias FROM nfe012 WHERE cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND CbdNtfNumero = '" + int.Parse(notafiscal).ToString() + "' AND CbdNtfSerie = '" + int.Parse(serie).ToString() + "'  AND CbdMod = '65' ";

            return  Conexao.CriarEntidade().ExecuteStoreQuery<nfe012E>(SQL).FirstOrDefault();
        }

        public nfe012E notafiscalDocumento(string Documento)
        {
            string SQL = "SELECT IFNULL(CbdEmpCodigo,0) AS CbdEmpCodigo,IFNULL(CbdNtfNumero,0) AS CbdNtfNumero,IFNULL(CbdNtfSerie,0) AS CbdNtfSerie,IFNULL(CbdAcao,'') AS CbdAcao,IFNULL(CbdSituacao,'') AS CbdSituacao, " +
                    "IFNULL(CbdDtaProcessamento, 0) AS CbdDtaProcessamento, IFNULL(CbdNumProtocolo,'') AS CbdNumProtocolo, IFNULL(CbdStsRetCodigo,'') AS CbdStsRetCodigo, IFNULL(CbdStsRetNome,'') AS CbdStsRetNome," +
                    "IFNULL(CbdProcStatus,'') AS CbdProcStatus, IFNULL(CbdNFEChaAcesso,'') AS CbdNFEChaAcesso, IFNULL(CbdxMotivo,'') AS CbdxMotivo, IFNULL(CbdXML,'') AS CbdXML," +
                    "IFNULL(CbdNumProtCanc,'') AS CbdNumProtCanc, IFNULL(CbdDtaCancelamento,'') AS CbdDtaCancelamento, IFNULL(CbdDtaInutilizacao,'') AS CbdDtaInutilizacao, IFNULL(CbdNumProtInut,'') AS CbdNumProtInut, " +
                    "IFNULL(CbdMarca,'') AS CbdMarca, IFNULL(CbdDigVal,'') AS CbdDigVal, IFNULL(cbdcodigofilial,'') AS cbdcodigofilial, IFNULL(cbdCCe,'') AS cbdCCe, IFNULL(cbdCCePath,'') AS cbdCCePath," +
                    "IFNULL(cbdCCenSeqEvento,'') AS cbdCCenSeqEvento, IFNULL(CbdMod,'') AS CbdMod, IFNULL(cbdDocumento,'') AS cbdDocumento, IFNULL(cbdNRec,'') AS cbdNRec," +
                    "IFNULL(cbdVerificado,'') AS cbdVerificado, IFNULL(CdbNtfNumeroFinal,'') AS CdbNtfNumeroFinal, IFNULL(Cbdflag,'') AS Cbdflag, IFNULL(count(1),0) as qtdNota, IFNULL(DATEDIFF(CURRENT_DATE, DATE(CbdDtaProcessamento)),0) AS dias FROM nfe012 WHERE cbdcodigofilial = '" + GlbVariaveis.glb_filial + "' AND cbddocumento = '" + Documento + "' AND CbdMod = '65' ";

            return Conexao.CriarEntidade().ExecuteStoreQuery<nfe012E>(SQL).FirstOrDefault();
        }

        public bool gerarXML(nfe012E nfe012)
        {
            if (nfe012.CbdStsRetCodigo == "0" && nfe012.CbdNumProtocolo == "0" && nfe012.CbdProcStatus == "O" && nfe012.CbdXML != "" && int.Parse(nfe012.dias) < 2)
                return false;
            else
                return true;
        }

        public string CbdEmpCodigo { get; set; }
        public string CbdNtfNumero { get; set; }
        public string CbdNtfSerie { get; set; }
        public string CbdAcao { get; set; }
        public string CbdSituacao { get; set; }
        public string CbdDtaProcessamento { get; set; }
        public string CbdNumProtocolo { get; set; }
        public string CbdStsRetCodigo { get; set; }
        public string CbdStsRetNome { get; set; }
        public string CbdProcStatus { get; set; }
        public string CbdNFEChaAcesso { get; set; }
        public string CbdxMotivo { get; set; }
        public string CbdXML { get; set; }
        public string CbdNumProtCanc { get; set; }
        public string CbdDtaCancelamento { get; set; }
        public string CbdDtaInutilizacao { get; set; }
        public string CbdNumProtInut { get; set; }
        public string CbdMarca { get; set; }
        public string CbdDigVal { get; set; }
        public string cbdcodigofilial { get; set; }
        public string cbdCCe { get; set; }
        public string cbdCCePath { get; set; }
        public string cbdCCenSeqEvento { get; set; }
        public string CbdMod { get; set; }
        public string cbdDocumento { get; set; }
        public string cbdNRec { get; set; }
        public string cbdVerificado { get; set; }
        public string CdbNtfNumeroFinal { get; set; }
        public string Cbdflag { get; set; }
        public string qtdNota { get; set; }
        public string dias { get; set; }
    }

}
