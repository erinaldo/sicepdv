using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    class FuncoesNF
    {
        

        public static void abrirCupom(Destinatario dados)
        {   
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFAbrir_NFCe_Daruma(dados.CPF, dados.nome, dados.endereco, dados.numero, dados.bairro, dados.codigoMunicipio.ToString(),dados.municipio,dados.UF,dados.UF);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno abrindo cupom " + retorno.ToString());
            }
        }

        public static void abrirCupomNumeroSerie(Destinatario dados, string numero, string serie)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFAbrirNumSerie_NFCe_Daruma(numero,serie,dados.CPF, dados.nome, dados.endereco, dados.numero, dados.bairro, dados.codigoMunicipio.ToString(), dados.municipio, dados.UF, dados.UF);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno abrindo numero serie cupom " + retorno.ToString());
            }
        }

        public static void VenderItem(ProdutoSimples dados)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFVender_NFCe_Daruma(dados.Aliquota, dados.quantidade.ToString(), dados.precoUnitario.ToString("N2"), dados.TipoDescAcresc.ToString(),dados.ValorDescAcresc.ToString("N2"), dados.codigoitem, dados.unidade, dados.descricao);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno vender item simples " + retorno.ToString());
            }
        }

        public static void VenderItemCompleto(ProdutoCompleto dados)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFVenderCompleto_NFCe_Daruma(dados.Aliquota, dados.quantidade.ToString(), dados.precoUnitario.ToString("N2"), dados.TipoDescAcresc, dados.ValorDescAcresc.ToString("N2"), dados.codigoitem, dados.NCM,dados.CFOP,dados.unidade, dados.descricao,"");
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno venda item completo " + retorno.ToString());
            }
        }


        public static bool CancelarItem(int Seguencia)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFCancelarItem_NFCe_Daruma(Seguencia.ToString());
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno cancelar item " + retorno.ToString());
                return false;
            }

            return true;
        }

        public static void Totalizar(TipoDescAcrescTotalizador tipoDescAcres, decimal ValorDescAcres)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFTotalizar_NFCe_Daruma(tipoDescAcres.ToString(), ValorDescAcres.ToString("N2"));
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno cancelar item " + retorno.ToString());
            }
        }

        public static void EfetuarPagamento(FormasPagamento formaPagamento, decimal Valor)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFEfetuarPagamento_NFCe_Daruma(formaPagamento.ToString(), Valor.ToString("N2"));
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno cancelar item " + retorno.ToString());
            }
        }

        public static void IdentificarConsumidor(Destinatario dados)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFIdentificarConsumidor_NFCe_Daruma(dados.CPF, dados.nome, dados.endereco, dados.numero, dados.bairro, dados.codigoMunicipio.ToString(), dados.municipio, dados.UF, dados.CEP, dados.Email);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno identificar consumidor " + retorno.ToString());
            }
        }

        public static void LeiImposto(decimal valor)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.aCFValorLeiImposto_NFCe_Daruma(valor.ToString("N2"));
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno lei de imposto " + retorno.ToString());
            }
        }

        public static void Encerrar(string mensagem = "")
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.tCFEncerrar_NFCe_Daruma(mensagem);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno encerrar " + retorno.ToString());
            }
        }

        public static void Cancelar(CancelarNF dados)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.tCFCancelar_NFCe_Daruma(dados.numero, dados.serie, dados.chaveAcesso, dados.protocoloAutorizacao, dados.justificativa);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno cancelar " + retorno.ToString());
            }
        }

        public static void inutilizar(Inutilizacao dados)
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.tCFInutilizar_NFCe_Daruma(dados.NumeroInicial, dados.NumeroFinal, dados.NumeroSerie, dados.Justificativa);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno inutilizacao " + retorno.ToString());
            }
        }

        public static void retonoInformacao(retornoInformacao dados)
        {
            int iRetorno;
            string retorno;
            StringBuilder resposta = new StringBuilder();
            iRetorno = Declaracoes.rRetornarInformacao_NFCe_Daruma(dados.TipoIntervalor.ToString(), dados.IndiceIncio, dados.IndiceFinal, dados.Serie, dados.ChaveAcesso, dados.InformecaoRetorno.ToString(), resposta);
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno informacao " + retorno.ToString());
            }
        }

        public static void VerificarStatusWSMigrate()
        {
            int iRetorno;
            string retorno;
            iRetorno = Declaracoes.rStatusWS_NFCe_Daruma();
            if (iRetorno != 1)
            {
                retorno = Declaracoes.TrataRetorno(iRetorno);
                throw new Exception("retorno status WS " + retorno.ToString());
            }
        }

        public static void RecuperarXML()
        {
           
        }

    }
}
