using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.EntityClient;
using System.Data;
using System.Data.Common;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using System.Data.Objects;
using System.Globalization;

namespace SICEpdv
{
    public class Produtos: IcadastroProduto
    {
        public string codigo { get; set; }
        public string codigoBarras { get; private set; }
        public string descricao { get; private set; }
        public decimal quantidade { get; private set; }
        public decimal quantidadeOrcamento { get; private set; }
        public decimal quantidadeDisponivel { get; private set; }
        public decimal quantidadePrat { get; private set; }
        public string situacao { get; private set; }
        public string tipo { get; set; }
        public decimal descontoPromocao { get; set; }
        public DateTime validadePromocao { get; set; }
        public decimal icms { get; private set; }
        public string tributacao { get; private set; }
        public string unidade { get; private set; }
        public int embalagem { get; private set; }
        public decimal preco { get; set; }
        public decimal custo { get; set; }
        public bool aceitaDesconto { get; set; }
        public decimal quantidadeMinDesconto { get; set; }
        public bool descricaoComplementar { get; set; }
        public string ncm { get; set; }
        public string nbm { get; set; }
        public string ncmespecie { get; set; }
        public decimal reducaoBaseCalcICMS { get; set; }
        public decimal ipi { get; set; }
        public string situacaoInventario { get; set; }
        public string grade { get; set; }
        public decimal descontoMaximo { get; set; }
        public static List<StandAloneProdutos> ProdutosOFF { get; set; }
        public decimal pis { get; set; }
        public decimal cofins { get; set; }
        public decimal aliquotaIPI { get; set; }
        public string classe { get; set; }
        public string cest { get; set; }
        public static string tabelaPreco { get; set; }
        public bool controleLoteProduto { get; set; }
        public string sincronizar { get; set; }
        

        /// <summary>
        /// Existem 2 preços o preço de atacado e o preço de varejo (normal)
        /// A variável tabelaPreço define qual o preço será usado
        /// </summary>


        public Produtos()
        {
            embalagem = 1;         
        }

        #region Procura
        public bool ProcurarCodigo(string codigoPesquisa,string filial,bool restricoes=true)
        {
            
                        /// If operandus modus = StandAlone
            #region StandAlone
            Conexao.stringConexao = null;

            int posicao = Configuracoes.posicaoCodBarrasBalanca-1;
            int tamanho = Configuracoes.tamanhacodBarrasBalanca;

            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                //   IObjectContainer tabela = Db4oFactory.OpenFile("produtos.yap");n
                try
                {                    
                    var dados = from StandAloneProdutos n in ProdutosOFF
                                where ( (n.codigo == codigoPesquisa )                                
                                || (( (n.codigoBarras == codigoPesquisa.PadLeft(14,' ').Substring(posicao, tamanho) && codigoPesquisa.Substring(0,1)==Configuracoes.digitoVerificadorCodBarras) && n.codigoBarras != "")
                                || (n.codigoBarras == codigoPesquisa)) )
                                && (n.codigoFilial == filial)
                                select n;



                    foreach (var item in dados)
                    {
                        codigo = item.codigo;
                        codigoBarras = item.codigoBarras;
                        descricao = item.descricao;
                        quantidade = item.quantidade;
                        quantidadeDisponivel = item.quantidadeDisponivel;
                        quantidadeOrcamento = item.qtdPrateleiras;
                        quantidadePrat = item.qtdPrateleiras;
                        situacao = item.situacao;                        
                        if (item.situacao == "Promoção" || item.situacao == "Promocao")
                        descontoPromocao = item.descontoPromocao;

                        icms = item.icms;
                        tributacao = item.tribuacao;
                        unidade = item.unidade;
                        embalagem = 1;                       
                        preco = item.preco;
                        descricaoComplementar = item.ativacompdesc == "S" ? true : false;
                        if (tabelaPreco == "atacado")
                        {
                            embalagem = item.embalagem;
                            preco = item.precoatacado;
                            unidade = item.unidadeembalagem;
                        }                        
                        tipo = item.tipo;
                        grade = item.grade;
                        descontoMaximo = item.descontoMaximo;
                        pis = item.pis;
                        cofins = item.cofins;
                        aliquotaIPI = item.aliquotaIPI; 
                    }

                    if (dados.Count() == 0)
                        throw new System.Exception("Código não encontrado");

                    //tabela.Close();
                    return Restricoes();
                }
                catch (Exception e)
                {
                    throw new System.Exception("Não foi possível fazer a pesquisa: "+e.Message.ToString());
                }

                
                //finally
                //{
                //    tabela.Close();
                //}

            };
            #endregion

            siceEntities entidade;
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();


            if (filial == "00001")
            {
                #region matriz

                var dados = (from n in entidade.produtos
                            where ((n.codigo == codigoPesquisa)
                            || (((n.codigobarras == codigoPesquisa.Substring(posicao, tamanho) && codigoPesquisa.Substring(0, 1) == Configuracoes.digitoVerificadorCodBarras) 
                            && n.codigobarras != "" && restricoes)
                            || (n.codigobarras == codigoPesquisa && restricoes )))
                            && (n.CodigoFilial == filial)
                            select new
                            {
                                n.codigo,
                                n.codigobarras,
                                n.descricao,
                                n.quantidade,
                                n.qtdprevenda,
                                n.qtdprateleiras,
                                n.situacao,
                                n.descontopromocao,
                                n.icms,
                                n.tributacao,
                                n.unidade,
                                n.precovenda,
                                n.precoatacado,
                                n.unidembalagem,
                                n.embalagem,
                                n.custo,
                                n.tipo,
                                n.aceitadesconto,
                                n.ativacompdesc,
                                n.ncm,
                                n.percentualRedBaseCalcICMS,
                                n.ipi,
                                n.aliquotaIPI,
                                n.nbm,
                                n.ncmespecie,
                                n.situacaoinventario,
                                n.grade,
                                n.descontomaximo,
                                n.qtdminimadesc,
                                n.pis,
                                n.cofins,
                                n.classe,
                                n.sincronizar,
                                n.validadepromoc                                    
                            }).FirstOrDefault();


                    if (dados == null)
                    {
                        throw new Exception(codigoPesquisa + " codigo não foi encontrado.");
                    }
                    else
                    {
                        cest = Produtos.codigocestProdutos(dados.codigo, filial);
                        controleLoteProduto = Produtos.Lote(dados.codigo, filial);
                    }

                    if (Produtos.ProdutosInventario(dados.codigo, filial) == "N")
                    {
                        throw new Exception(codigoPesquisa + " Codigo não Permitido para venda no PDV.");
                    }


                    codigo = dados.codigo;
                    codigoBarras = dados.codigobarras;
                    descricao = dados.descricao;
                    quantidade = dados.quantidade;

                    decimal quantidadeGrupo = 0;
                    string somarGrupo = "N";
                    try
                    {
                        somarGrupo = Conexao.CriarEntidade().ExecuteStoreQuery<String>("SELECT IFNULL(somarestoquegrupo,'N') AS somarestoquegrupo FROM produtos WHERE codigo = '" + codigo + "' limit 1").FirstOrDefault();

                        if (somarGrupo == "S")
                        {
                        //quantidadeGrupo = Conexao.CriarEntidade().ExecuteStoreQuery<Decimal>("SELECT IFNULL(SUM(p.quantidade + pf.quantidade),0) FROM produtos AS p, produtosfilial AS pf WHERE p.codigo = '" + codigo + "' AND p.codigo = pf.codigo limit 1").FirstOrDefault();
                          quantidadeGrupo = Conexao.CriarEntidade().ExecuteStoreQuery<Decimal>("SELECT IFNULL(quantidade+qtdemtransito+(SELECT SUM(quantidade+qtdemtransito) FROM produtosfilial WHERE codigo = '" + codigo + "' AND codigoFilial IN (SELECT codigoFilial FROM filiais WHERE ativa = 'S')),0) AS filiais FROM produtos WHERE codigo = '" + codigo + "'").FirstOrDefault();
                        }
                    }
                    catch (Exception erro)
                    {
                       
                    }

                    if(somarGrupo == "N")
                        quantidadeDisponivel = (dados.quantidade - (dados.qtdprevenda < 0 ? dados.qtdprevenda * -1 : dados.qtdprevenda));
                    else
                        quantidadeDisponivel = (quantidadeGrupo - (dados.qtdprevenda < 0 ? dados.qtdprevenda * -1 : dados.qtdprevenda));

                    quantidadeOrcamento = dados.qtdprevenda;
                    quantidadePrat = dados.qtdprateleiras;
                    situacao = dados.situacao;
                    if (dados.situacao == "Promoção" || dados.situacao == "Promocao")
                        descontoPromocao = dados.descontopromocao;
                    else
                        descontoPromocao = 0;

                    icms = dados.icms;
                    tributacao = dados.tributacao;
                    unidade = dados.unidade;
                    embalagem = 1;
                    preco = dados.precovenda;
                    if (tabelaPreco == "atacado")
                    {
                        preco = dados.precoatacado;
                        unidade = dados.unidembalagem;
                        embalagem = dados.embalagem;
                    }
                    custo = dados.custo.Value;
                    tipo = dados.tipo;
                    aceitaDesconto = dados.aceitadesconto == "S" ? true : false;
                    descricaoComplementar = dados.ativacompdesc == "S" ? true : false;
                    ncm = dados.ncm;
                    reducaoBaseCalcICMS = dados.percentualRedBaseCalcICMS;
                    ipi = dados.ipi;
                    nbm = dados.nbm;
                    ncmespecie = dados.ncmespecie;
                    situacaoInventario = dados.situacaoinventario;
                    grade = dados.grade;
                    descontoMaximo = dados.descontomaximo;
                    quantidadeMinDesconto = dados.qtdminimadesc;
                    pis = dados.pis.GetValueOrDefault();
                    cofins = dados.cofins.Value;
                    aliquotaIPI = dados.aliquotaIPI.GetValueOrDefault();
                    classe = dados.classe;
                    sincronizar = dados.sincronizar;
                    validadePromocao = dados.validadepromoc == null ? new DateTime(1900,01,01): dados.validadepromoc.Value;  

                #endregion
            }
            else
            {
                #region filial

                var dados2 = (from n in entidade.produtosfilial
                                  where ((n.codigo == codigoPesquisa)
                                  || (((n.codigobarras == codigoPesquisa.Substring(posicao, tamanho) && codigoPesquisa.Substring(0, 1) == Configuracoes.digitoVerificadorCodBarras) && n.codigobarras != "" && restricoes)
                                  || (n.codigobarras == codigoPesquisa && restricoes)))
                                  && (n.CodigoFilial == filial)
                                  select new
                                  {
                                      n.codigo,
                                      n.codigobarras,
                                      n.descricao,
                                      n.quantidade,
                                      n.qtdprateleiras,
                                      n.qtdprevenda,
                                      n.situacao,
                                      n.descontopromocao,
                                      n.icms,
                                      n.tributacao,
                                      n.unidade,
                                      n.precovenda,
                                      n.precoatacado,
                                      n.unidembalagem,
                                      n.embalagem,
                                      n.custo,
                                      n.tipo,
                                      n.aceitadesconto,
                                      n.ativacompdesc,
                                      n.ncm,
                                      n.percentualRedBaseCalcICMS,
                                      n.ipi,
                                      n.aliquotaIPI,
                                      n.nbm,
                                      n.ncmespecie,
                                      n.situacaoinventario,
                                      n.grade,
                                      n.descontomaximo,
                                      n.qtdminimadesc,
                                      n.pis,
                                      n.cofins,
                                      n.classe,
                                      n.sincronizar,
                                      n.validadepromoc
                                  }).FirstOrDefault();

                    if (dados2 == null)
                    {
                        throw new System.Exception(codigoPesquisa + " Código não encontrado");
                    }
                    else
                    {
                        cest = Produtos.codigocestProdutos(dados2.codigo, filial);
                        controleLoteProduto = Produtos.Lote(dados2.codigo, filial);
                    }


                    codigo = dados2.codigo;
                    codigoBarras = dados2.codigobarras;
                    descricao = dados2.descricao;
                    quantidade = dados2.quantidade;
                    quantidadeOrcamento = dados2.qtdprevenda;
                //quantidadeDisponivel = (dados2.quantidade - dados2.qtdprevenda);

                    decimal quantidadeGrupo = 0;
                    string somarGrupo = "N";
                    try
                    {
                        somarGrupo = Conexao.CriarEntidade().ExecuteStoreQuery<String>("SELECT IFNULL(somarestoquegrupo,'N') AS somarestoquegrupo FROM produtos WHERE codigo = '" + codigo + "' limit 1").FirstOrDefault();

                        if (somarGrupo == "S")
                        {
                            quantidadeGrupo = Conexao.CriarEntidade().ExecuteStoreQuery<Decimal>("SELECT IFNULL(SUM(p.quantidade + pf.quantidade),0) FROM produtos AS p, produtosfilial AS pf WHERE p.codigo = '" + codigo + "' AND p.codigo = pf.codigo limit 1").FirstOrDefault();
                        }
                    }
                    catch (Exception erro)
                    {

                    }

                    if (somarGrupo == "N")
                        quantidadeDisponivel = (dados2.quantidade - (dados2.qtdprevenda < 0 ? dados2.qtdprevenda * -1 : dados2.qtdprevenda));
                    else
                        quantidadeDisponivel = (quantidadeGrupo - (dados2.qtdprevenda < 0 ? dados2.qtdprevenda * -1 : dados2.qtdprevenda));

                    //quantidadeDisponivel = (dados2.quantidade - (dados2.qtdprevenda < 0 ? dados2.qtdprevenda * -1 : dados2.qtdprevenda));

                    quantidadePrat = dados2.qtdprateleiras;
                    situacao = dados2.situacao;
                    if (dados2.situacao == "Promoção" || dados2.situacao == "Promocao")
                        descontoPromocao = dados2.descontopromocao;
                    icms = dados2.icms;
                    tributacao = dados2.tributacao;
                    unidade = dados2.unidade;
                    embalagem = 1;
                    preco = dados2.precovenda;
                    aceitaDesconto = dados2.aceitadesconto == "S" ? true : false;
                    descricaoComplementar = dados2.ativacompdesc == "S" ? true : false;
                    if (tabelaPreco == "atacado")
                    {
                        embalagem = dados2.embalagem.Value;
                        preco = dados2.precoatacado;
                        unidade = dados2.unidembalagem;
                    }
                    custo = dados2.custo.Value;
                    tipo = dados2.tipo;
                    ncm = dados2.ncm;
                    reducaoBaseCalcICMS = dados2.percentualRedBaseCalcICMS;
                    ipi = dados2.ipi;
                    nbm = dados2.nbm;
                    ncmespecie = dados2.ncmespecie;
                    situacaoInventario = dados2.situacaoinventario;
                    grade = dados2.grade;
                    descontoMaximo = dados2.descontomaximo;
                    quantidadeMinDesconto = dados2.qtdminimadesc;
                    pis = dados2.pis.GetValueOrDefault();
                    cofins = dados2.cofins.GetValueOrDefault();
                    aliquotaIPI = dados2.aliquotaIPI.GetValueOrDefault();
                    classe = dados2.classe;
                    sincronizar = dados2.sincronizar;
                    validadePromocao = dados2.validadepromoc == null ? new DateTime(1900, 01, 01) : dados2.validadepromoc.Value;

                #endregion
            }
           
            if (restricoes)                
            return Restricoes();

            return true;

        }
        #endregion Procura

        private bool Restricoes()
        {
            
           if (situacao== "Excluído" || situacao == "Inativo")
               throw new System.Exception("Produto com Situação: " + situacao.ToUpper());

           if (string.IsNullOrEmpty(unidade))
               throw new System.Exception("Produto sem unidade");

           if (string.IsNullOrEmpty(descricao))
               throw new System.Exception("Produto sem descrição");

            if (string.IsNullOrEmpty(codigo))
                throw new System.Exception("Código vazio");

            if (tipo.Substring(0, 1) == "7" || tipo.Substring(0,1) == "1" || tipo.Substring(0, 1) == "8")
                throw new System.Exception("Não é permitido vender produtos do Tipo.: "+tipo.ToString());

            //if (dados.Count() == 0)
            //    throw new System.Exception("Código não encontrado");


            return true;
        }

        public static string codigocestProdutos(string codigo, string codigofilial)
        {
            try
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();


                if (codigofilial == "00001")
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT IF(IFNULL(cest,0) = '',0,IFNULL(cest,0)) AS cest FROM produtos WHERE codigo = '" + codigo + "' limit 1").FirstOrDefault();
                }
                else
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT IF(IFNULL(cest,0) = '',0,IFNULL(cest,0)) AS cest FROM produtosfilial WHERE codigo = '" + codigo + "' AND codigofilial ='" + codigofilial + "' limit 1").FirstOrDefault();
                }
            }
            catch (Exception erro)
            {
                return "0";
            }
        }

        public static string ProdutosInventario(string codigo, string codigofilial)
        {
            try
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();


                if (codigofilial == "00001")
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT produtoInventario FROM produtos WHERE codigo = '" + codigo + "' limit 1").FirstOrDefault();
                }
                else
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT produtoInventario FROM produtosfilial WHERE codigo = '" + codigo + "' AND codigofilial ='" + codigofilial + "' limit 1").FirstOrDefault();
                }
            }
            catch (Exception erro)
            {
                return "S";
            }
        }

        public static bool Lote(string codigo, string codigofilial)
        {
            try
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();


                if (codigofilial == "00001")
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT IFNULL(controlelote,'N') AS lote FROM produtos WHERE codigo = '" + codigo + "' limit 1").FirstOrDefault() == "S" ? true : false ;
                }
                else
                {
                    return entidade.ExecuteStoreQuery<String>("SELECT IFNULL(controlelote,'N') AS lote FROM produtosfilial WHERE codigo = '" + codigo + "' AND codigofilial ='" + codigofilial + "' limit 1").FirstOrDefault() == "S" ? true : false;
                }
            }
            catch (Exception erro)
            {
                return false;
            }
        }

        public static List<UnidadeMedidas> Unidades()
        {        
            
            var dados = new[]
            {
            new UnidadeMedidas {unidade="UNI",descricao="UNIDADE"},            
            new UnidadeMedidas {unidade="UN",descricao="UNIDADE"},
            new UnidadeMedidas {unidade="CX",descricao="CAIXA"},
            new UnidadeMedidas {unidade="CON",descricao="CONE"},
            new UnidadeMedidas {unidade="G",descricao="GRAMA"},
            new UnidadeMedidas {unidade="GR",descricao="GRADE"},
            new UnidadeMedidas {unidade="DZ",descricao="DÚZIA"},
            new UnidadeMedidas {unidade="KG",descricao="QUILO"},
            new UnidadeMedidas {unidade="L",descricao="LITRO"},            
            new UnidadeMedidas {unidade="M",descricao="METRO"},
            new UnidadeMedidas {unidade="MIL",descricao="MILHEIRO"},
            new UnidadeMedidas {unidade="PC",descricao="PEÇA"},
            new UnidadeMedidas {unidade="FR",descricao="FRAÇÃO"},
            new UnidadeMedidas {unidade="P",descricao="PAR"},
            new UnidadeMedidas {unidade="SC",descricao="SACO"},
            new UnidadeMedidas {unidade="CJ",descricao="CONJUNTO"},
            new UnidadeMedidas {unidade="TN",descricao="TONELADA"},
            new UnidadeMedidas {unidade="BB",descricao="BOMBONA"},
            new UnidadeMedidas {unidade="BD",descricao="BALDE"},
            new UnidadeMedidas {unidade="GL",descricao="GALÃO"},
            new UnidadeMedidas {unidade="LT",descricao="LATÃO"},
            new UnidadeMedidas {unidade="M2",descricao="METRO QUADRADO"},
            new UnidadeMedidas {unidade="M3",descricao="METRO CÚBICO"},
            new UnidadeMedidas {unidade="RL",descricao="ROLO"},
            new UnidadeMedidas {unidade="CJ",descricao="CONJUNTO"},
            new UnidadeMedidas {unidade="PAR",descricao="PARES"},
            new UnidadeMedidas {unidade="JG",descricao="JOGO"},            
            new UnidadeMedidas {unidade="ROL",descricao="ROLO"},
            new UnidadeMedidas {unidade="PAC",descricao="PACOTE"},
            new UnidadeMedidas {unidade="ML",descricao="MILITRO"},
            new UnidadeMedidas {unidade="MM",descricao="MILIMETRO"},
            };

            return dados.ToList();
        }

        public static bool GravarComposicao(string codigoPrd, string codMateria, decimal quantidade, string unidade)
        {
            if (Conexao.ConexaoOnline())
            {
                siceEntities entidade = Conexao.CriarEntidade();
                //if (Conexao.tipoConexao == 2)
                //entidade = Conexao.CriarEntidade(false);


                if (codigoPrd == codMateria)
                    throw new Exception("Código do produto não pode ser igual ao código da matéria prima");
                if (quantidade <= 0)
                    throw new Exception("Digite uma quantidade válida");
                if (string.IsNullOrEmpty(unidade))
                    unidade = "UN";

                var dadosProdutos = (from n in Conexao.CriarEntidade().produtos
                                     where n.codigo == codigoPrd
                                     && n.CodigoFilial == GlbVariaveis.glb_filial
                                     select new { n.descricao });

                if (dadosProdutos.Count() == 0)
                    throw new Exception("Código não encontrado");


                if (string.IsNullOrEmpty(dadosProdutos.FirstOrDefault().descricao))
                    throw new Exception("Código não encontrado");

                var dadosMateria = (from n in Conexao.CriarEntidade().produtos
                                    where n.codigo == codMateria
                                    && n.CodigoFilial == GlbVariaveis.glb_filial
                                    select new { n.descricao, n.custo });
                if (dadosMateria.Count() == 0)
                    throw new Exception("Código não encontrado");

                produtoscomposicao prd = new produtoscomposicao();
                prd.codigo = codigoPrd;
                prd.codigofilial = GlbVariaveis.glb_filial;
                prd.codigomateria = codMateria;
                prd.custo = dadosMateria.First().custo;
                prd.custototal = dadosMateria.First().custo.Value * quantidade;
                prd.descricao = dadosProdutos.First().descricao;
                prd.descricaomateria = dadosMateria.First().descricao;
                prd.unidade = unidade.Split("-".ToCharArray()).First().Substring(0, 2);
                prd.quantidade = quantidade;
                entidade.AddToprodutoscomposicao(prd);
                entidade.SaveChanges();
            }
            else
            {
                MessageBox.Show("Não é permitido PDV em modo OFF-line");
            }
            return true;
        }

        public static bool ApagarComposicao(string codigoPrd, string codigoMateria)
        {
            if (Conexao.ConexaoOnline())
            {
                siceEntities entidade = Conexao.CriarEntidade();
                //if (Conexao.tipoConexao == 2)
                //entidade = Conexao.CriarEntidade(false);

                produtoscomposicao excluir = (from n in entidade.produtoscomposicao
                                              where n.codigo == codigoPrd && n.codigomateria == codigoMateria
                                              && n.codigofilial == GlbVariaveis.glb_filial
                                              select n).First();

                entidade.DeleteObject(excluir);
                entidade.SaveChanges();
            }
            else
            {
                MessageBox.Show("Não é permitido PDV em modo OFF-line");
            }

            return true;
        }

        public static int ParcelamentoMaxVenda()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            string tabela = "produtos";
            if (GlbVariaveis.glb_filial != "00001")
                tabela = "produtosfilial";

            string sql = @"SELECT MAX(" + tabela + ".parcelamentomax) FROM " + tabela + ",vendas " +
                         "WHERE " + tabela + ".codigofilial=" + "'" + GlbVariaveis.glb_filial + "'" +
                         "and " + tabela + ".codigo=vendas.codigo AND vendas.id=" + "'" + GlbVariaveis.glb_IP + "'";

            int parcelaPrd = entidade.ExecuteStoreQuery<int>(sql).FirstOrDefault();
            return parcelaPrd;
        }

        public static IQueryable<vendas> PermissaoClasseVenda(string classe)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            string sql = "SELECT * from vendas WHERE id=" + "'" + GlbVariaveis.glb_IP + "'" +
                "AND classe <>'" + classe + "' AND classe<>'0000' AND cancelado='N'";

            var dados = entidade.ExecuteStoreQuery<vendas>(sql).AsQueryable();
            return dados;
        }

        public string CadastrarProduto(produtos dados)
        {
            try
            {
                ValidarCampos(dados);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            throw new NotImplementedException();
        }

        public string AlterarProduto(produtos dados)
        {
            throw new NotImplementedException();
        }

        public bool ValidarCampos(produtos dados)
        {

            try
            {
                if(string.IsNullOrEmpty(dados.ncm) || string.IsNullOrEmpty(dados.ncmespecie))
                {
                    throw new Exception("NCM e NCM espécie não pode ser vazio");
                }

                if (dados.ncm.Length<8)
                {
                    throw new Exception("NCM tem que ter 8 ou mais caracteres");
                }


                if (string.IsNullOrEmpty(dados.descricao))
                {
                    throw new Exception("Descrição não pode ser vazia.");
                }

                if (string.IsNullOrEmpty(dados.grupo) || string.IsNullOrEmpty(dados.subgrupo ))
                {
                    throw new Exception("Verifique grupo e subgrupo");
                }
                
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public decimal FormarPreco(decimal custo, decimal margem)
        {
            throw new NotImplementedException();
        }

        public bool VerificarTributacao(produtos dados)
        {
            throw new NotImplementedException();
        }

        static public decimal tabelaPrecoQtd(string codigo, string tipoPreco, decimal quantidade)
        {
            try
            {

                siceEntities entidade;
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                if (tipoPreco == "varejo" || tipoPreco == null)
                    tipoPreco = "V";
                else
                    tipoPreco = "A";


                string SQL = "SELECT IFNULL(tabelapreco('" + codigo + "','" + GlbVariaveis.glb_filial + "','" + tipoPreco + "','" + quantidade.ToString("N2", CultureInfo.CreateSpecificCulture("en-US")) + "','T'),0)";

                return entidade.ExecuteStoreQuery<decimal>(SQL).FirstOrDefault();
            }
            catch (Exception)
            {
                return 0;
            }
           
            
        }

        public List<ServiceProdutos.ProdutosContador> ListagemProdutosContadorLocal(string filtro="")
        {

            string tabela = "produtos";
            if (GlbVariaveis.glb_filial != "00001")
                tabela = "produtosfilial";



            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            string sql ="SELECT codigo,codigobarras,descricao,ncm,origem,tributacao,icms,cest,cfopsaida,situacaoinventario,tributacaopis,tributacaocofins,pis,cofins,codigosuspensaopis,codigosuspensaocofins FROM " + tabela + " WHERE (situacao='Normal') and codigofilial='"+GlbVariaveis.glb_filial+"'" + filtro +" GROUP BY codigo";

            var dados = entidade.ExecuteStoreQuery<ServiceProdutos.ProdutosContador>(sql).AsQueryable();

            return dados.ToList();

        }

        public List<ServiceProdutos.ProdutosContador> ListagemProdutosContadorNuvem(string filtro = "")
        {
            try
            {
                ServiceProdutos.WSProdutosClient WSProdutos = new ServiceProdutos.WSProdutosClient();
                var dados = WSProdutos.ListagemProdutosContadorCNPJ(Configuracoes.cnpj, "", 9999999);
                return dados.ToList();
            }
            catch (Exception)
            {
                return null;
            }
           

        }



        public bool AtualizarProdutosContadorNuvem()
        {


            var listagemProdutos = ListagemProdutosContadorLocal();            
            var listagemNuvem = ListagemProdutosContadorNuvem();


            try
            {
                ServiceProdutos.WSProdutosClient WSProdutos = new ServiceProdutos.WSProdutosClient();
                try
                {
                    //Aqui Cadastra o contador 
                    if (!string.IsNullOrEmpty(Configuracoes.emailContador))
                    {
                        WSProdutos.IncluirContador(GlbVariaveis.chavePrivada,Convert.ToInt16(GlbVariaveis.idCliente), GlbVariaveis.glb_filial, Configuracoes.nomeContador, Configuracoes.emailContador, Configuracoes.crccontador, Configuracoes.cnpj, Configuracoes.razaoSocial, Configuracoes.fantasia, Configuracoes.tipoEmpresa,Configuracoes.cnae);
                    }
                }
                catch 
                {
                    
                }

                //Aqui fay uma instancia de apenas 100 itens para nao dar erro de sobrecarga
                int x = 0;
                List<ServiceProdutos.ProdutosContador> listaEnvio = new List<ServiceProdutos.ProdutosContador>();
                foreach (var item in listagemProdutos)
                {
                    var verificarInclusao = (from n in listagemNuvem where n.codigo == item.codigo select n).FirstOrDefault();

                    if (verificarInclusao == null || (item.icms!=verificarInclusao.icms || item.tributacao!=verificarInclusao.tributacao || item.ncm!=verificarInclusao.ncm || item.cest!=verificarInclusao.cest || item.tributacaopis!=verificarInclusao.tributacaopis || item.pis!=verificarInclusao.pis || item.tributacaocofins!=verificarInclusao.tributacaocofins || item.cofins!=verificarInclusao.cofins ))
                    {
                        listaEnvio.Add(item);
                    }

                    
                    if (x >= 100 && listaEnvio.Count() > 0)
                    {                       
                        WSProdutos.IncluirProdutosContador(GlbVariaveis.chavePrivada, GlbVariaveis.glb_filial, Configuracoes.cnpj, Configuracoes.razaoSocial, Configuracoes.fantasia,Configuracoes.tipoEmpresa, listaEnvio.ToArray());
                        listaEnvio.Clear();
                        x = 0;
                    }
                    x++;
                }

                // Aqui faz o envio do restante caso a lista seja menor que 100;

                if (listaEnvio.Count() > 0)
                {                 
                    WSProdutos.IncluirProdutosContador(GlbVariaveis.chavePrivada, GlbVariaveis.glb_filial, Configuracoes.cnpj, Configuracoes.razaoSocial, Configuracoes.fantasia, Configuracoes.tipoEmpresa, listaEnvio.ToArray());
                    listaEnvio.Clear();
                    x = 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("aqui" + ex.Message);
            }

        }

        public bool ApagarProdutosContador()
        {
            try
            {
                ServiceProdutos.WSProdutosClient WSProdutos = new ServiceProdutos.WSProdutosClient();
                return WSProdutos.ExcluirProdutosContador(GlbVariaveis.chavePrivada, Configuracoes.cnpj);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }

    public struct UnidadeMedidas
    {
        public string unidade { get; set; }
        public string descricao { get; set; }
    }


    public class listPesquisaProdutos
    {
        public string codigo { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public string STecf { get; set; }
        public string icms { get; set; }
        public string quantidade { get; set; }
        public string preco { get; set; }
        public string IAT { get; set; } 
        public string qtdDisponivel { get; set; }
        public string qtdprateleiras { get; set; }
        public string deposito { get; set; }
        public string IPPT { get; set; }
        public string filiais { get; set; }
        public string precoMinimo { get; set; }
        public string codigobarras { get; set; }
        public string localestoque { get; set; }
        public string impulsionarVendas { get; set; }
        public string situacao { get; set; }
        public string urlImagem { get; set; }
    }

    // Essa classe não deve ser modificado por que é usada no WebServer com as mesmas 
    //posições dos campos e nomes
 


}
