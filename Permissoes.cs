using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace SICEpdv
{
    public static class Permissoes
    {
        /// <summary>
        /// Permisões dos usuário para venda PDV
        /// </summary>
        public static bool administrador { get; set; }
        public static bool descontoFinalizacao { get; set; }
        public static bool arredondar { get; set; }
        public static bool venderQtdNegativa { get; set; }
        public static bool mudarVencimento { get; set; }
        public static bool operadorCaixa { get; set; }
        public static bool excluirDocumento { get; set; }
        public static bool lancarItemduplos { get; set; }
        public static bool excluirItemVenda { get; set; }
        public static bool alterarParcelamento { get; set; }
        public static bool venderClienteRestricao { get; set; }
        public static bool verSaldoCaixa { get; set; }
        public static bool reimprimirDocumento { get; set; }
        public static bool vendaatacado { get; set; }
        public static bool iniciarSaldoSuprimento { get; set; }
        public static bool fechamentocaixadigitacao { get; set; }
        public static bool iqsCloud { get; set; }
        // Recebimento
        public static bool receberContas { get; set; }

        public static void Carregar(string operador)
        {
            /// If operandus modus = StandAlone
            #region StandAlone
            if (!Conexao.onLine)
            {
                IObjectContainer tabela = Db4oFactory.OpenFile("senhas.yap");

                var permissaoOff = from StandAloneUsuario p in tabela
                                where p.operador == operador
                                select p;
                foreach (var item in permissaoOff)
                {
                    descontoFinalizacao = item.descontoFinalizacao;
                    arredondar = item.arredondar;
                    excluirDocumento = item.excluirDocumento;
                    operadorCaixa = item.operadorCaixa;
                    venderQtdNegativa = item.venderEstoqueNegativo;
                    excluirItemVenda = item.vendexcluiritem;
                    vendaatacado = item.vendaatacado;
                    verSaldoCaixa = true;
                    vendaatacado = false;
                    iniciarSaldoSuprimento = true;
                }

                tabela.Close();
                return;
            };
            #endregion StandAlone

            siceEntities entidade;
            try
            {
                entidade = Conexao.CriarEntidade();
                var teste = (from p in entidade.configfinanc
                             where p.CodigoFilial == GlbVariaveis.glb_filial
                             select p).ToList();
            }
            catch (Exception)
            {
                entidade = Conexao.CriarEntidade(false);
                var teste = (from p in entidade.configfinanc
                             where p.CodigoFilial == GlbVariaveis.glb_filial
                             select p).ToList();
            }

            try
            {
                var permissao = from p in entidade.senhas
                                where p.operador == operador
                                select p;
                foreach (var item in permissao)
                {
                    administrador = item.administrador == "S" ? true : false;
                    descontoFinalizacao = item.vendesconto == "S" ? true : false;
                    arredondar = item.vendarredondamento == "S" ? true : false;
                    venderQtdNegativa = item.estnegativo == "S" ? true : false;
                    mudarVencimento = item.vendaaltvenc == "S" ? true : false;
                    operadorCaixa = item.rotcaixa == "S" ? true : false;
                    excluirDocumento = item.venexcluir == "S" ? true : false;
                    receberContas = item.clireceber == "S" ? true : false;
                    lancarItemduplos = item.venitenscodigoduplos == "S" ? true : false;
                    excluirItemVenda = item.vendexcluiritem == "S" ? true : false;
                    alterarParcelamento = item.clialterarvencimento == "S" ? true : false;
                    venderClienteRestricao = item.clirestricao == "S" ? true : false;
                    verSaldoCaixa = item.relcaixa == "S" ? true : false;
                    vendaatacado = item.vendaatacado == "S" ? true : false;
                    iniciarSaldoSuprimento = item.rotaltersaldo == "S" ? true : false;
                    fechamentocaixadigitacao = item.fechamentocaixadigitacao == "S" ? true : false;
                    reimprimirDocumento = item.outimpcupom == "S" ? true : false;
                }

                try
                {
                    iqsCloud = Conexao.CriarEntidade().ExecuteStoreQuery<string>("SELECT iqscloud FROM senhas WHERE operador='" + operador + "' LIMIT 1").FirstOrDefault() == "S" ? true : false;
                }
                catch (Exception)
                {
                    iqsCloud = false;
                }
               
            }
            catch (Exception erro)
            {
                throw new Exception("Nenhuma configuração encontrada para a Operador: "+GlbVariaveis.glb_Usuario +" - "+ erro.ToString());
            }
        }
    }

    
}
