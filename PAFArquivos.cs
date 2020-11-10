using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace SICEpdv
{
    class PAFArquivos
    {
        #region N1 - Cabecalho
        public StringBuilder RegistroN1(string arquivoRegistro)
        {
            siceEntities entidade = Conexao.CriarEntidade();

            string sql = "SELECT `cnpjdesenvolvedora` AS cnpj,`inscricaodesenvolvedora` AS inscricao,`inscricaomunicipaldesenvolvedora` AS inscricaoMunicipal,`razaosocialdesenvolvedora` AS razao, MD5 AS chave FROM N1 LIMIT 1";

            var dados = Conexao.CriarEntidade().ExecuteStoreQuery<N1>(sql).FirstOrDefault();

            string razaoSocial = dados.razao.ToString();

            if (dados.chave != Funcoes.CriptografarMD5(dados.cnpj + dados.inscricao + dados.inscricaoMunicipal + dados.razao))
            {
                razaoSocial = razaoSocial + "??????????";
            }

            StringBuilder conteudo = new StringBuilder();
            conteudo.AppendLine("N1" +
                dados.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                dados.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                dados.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                razaoSocial);

            return conteudo;
        }
        #endregion

        #region U1 - Cabecalho
        public StringBuilder RegistroU1(string arquivoRegistro)
        {
            LogSICEpdv.Registrarlog("RegistroU1(" + arquivoRegistro + ")", "", "PAFArquivos.cs");

            siceEntities entidade = Conexao.CriarEntidade();
            #region

            var razao = "";

            if (!FuncoesPAFECF.VerificarQtdRegistro("vendaarquivo"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("vendadav"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            //aqui Implementar r01

            if (!FuncoesPAFECF.VerificarQtdRegistro("r01"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("r02"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("r03"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("produtos"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }


            if (!FuncoesPAFECF.VerificarQtdRegistro("caixaarquivo"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("contrelatoriogerencial"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("contdocs"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("contdav"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            if (!FuncoesPAFECF.VerificarQtdRegistro("filiais"))
            {
                razao = razao.Trim().PadRight(50, '?');
            }

            
            if (arquivoRegistro != "" && !FuncoesPAFECF.VerificarQtdRegistro(arquivoRegistro))
            {
                razao = razao.Trim().PadRight(50, '?');
            }
            #endregion

            string sql = "SELECT `cnpj` AS cnpj, `inscricao` AS inscricao, `inscricaomunicipal` AS inscricaoMunicipal, `empresa` AS razao, eaddados AS chave FROM filiais WHERE codigofilial = '"+GlbVariaveis.glb_filial+"' LIMIT 1";

            var dados = Conexao.CriarEntidade().ExecuteStoreQuery<N1>(sql).FirstOrDefault();

            string razaoSocial = dados.razao.ToString();

            if (dados.chave != Funcoes.CriptografarMD5(dados.cnpj + dados.inscricao + dados.inscricaoMunicipal + dados.razao) || razao != "")
            {
                razaoSocial = razaoSocial + razao + "??????????";
            }

            StringBuilder conteudo = new StringBuilder();
                conteudo.AppendLine("U1" +
                dados.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                dados.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                dados.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                razaoSocial);

          

            return conteudo;
        }
        #endregion

        #region A2
        public StringBuilder A2(DateTime dataInicial, DateTime dataFinal)
        {
            LogSICEpdv.Registrarlog("A2(" + dataInicial + "," + dataFinal + ")", "", "PAFArquivos.cs");

            decimal dinheiro = 0;
            decimal dinheiroRec = 0;
            decimal cheque = 0;
            decimal crediario = 0;
            decimal ticket = 0;
            decimal cartaoDB = 0;
            decimal cartaoCR = 0;
            decimal outros = 0;
            decimal outrosQ = 0;

            FuncoesECF fecf = new FuncoesECF();
            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;


            var pagamentos = (from c in entidade.caixaarquivo
                              where c.data >= dataInicial.Date && c.data <= dataFinal.Date && c.CodigoFilial == GlbVariaveis.glb_filial && c.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                              && c.estornado == "N"
                              //&& c.valor>0
                              select new
                              {
                                  c.tipopagamento,
                                  c.valor,
                                  c.data,
                                  c.vencimento,
                                  c.historico,
                                  c.coo,
                                  c.ecffabricacao,
                                  c.ccf,
                                  c.gnf,
                                  c.ecfmodelo,
                                  c.eaddados,
                                  c.dpfinanceiro,
                                  c.ocorrencia,
                                  c.tipodoc
                              });


            LogSICEpdv.Registrarlog("select * from caixa ", "", "FuncoesPAFECF.cs");

            var pagamentosDia = (from c in entidade.caixa
                                 where c.data >= dataInicial.Date && c.data <= dataFinal.Date && c.CodigoFilial == GlbVariaveis.glb_filial && c.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 && c.estornado == "N"
                                 //&& c.valor>0
                                 select new
                                 {
                                     c.tipopagamento,
                                     c.valor,
                                     c.data,
                                     c.vencimento,
                                     c.historico,
                                     c.coo,
                                     c.ecffabricacao,
                                     c.ccf,
                                     c.gnf,
                                     c.ecfmodelo,
                                     c.eaddados,
                                     c.dpfinanceiro,
                                     c.ocorrencia,
                                     c.tipodoc
                                 });

            var listaPagamento = pagamentosDia.ToList();


            if (pagamentos != null)
                listaPagamento = listaPagamento.Concat(pagamentos).ToList();

            var agrpData = (from n in listaPagamento
                            select n.data).Distinct();

            StringBuilder conteudo = new StringBuilder();

            #region
            //conteudo = RegistroU1("");


            //foreach (var item in listaPagamento)
            //{
            //    string tipoDoc = "1";
            //    if (item.coo == null)
            //        tipoDoc = "2";

            //    string meioPagamento = item.tipopagamento.Trim().PadRight(25, ' ').Substring(0, 25);
            //    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.coo + item.ccf + item.gnf + item.ecfmodelo + item.valor.ToString().Replace(",", ".") + item.tipopagamento);

            //    if (cripto != item.eaddados)
            //    {
            //        meioPagamento = meioPagamento.Trim().PadRight(25, '?');
            //    }

            //    conteudo.AppendLine("A2" +
            //    string.Format("{0:yyyyMMdd}", item.data) +
            //    meioPagamento +
            //    tipoDoc +
            //    Funcoes.FormatarZerosEsquerda(item.valor, 12, true));
            //}
            #endregion

            foreach (var item in agrpData)
            {

                #region
                /*
                dinheiro = (from n in listaPagamento
                           where n.tipopagamento == "DH" || n.tipopagamento=="XX"
                           && n.data == item.Value
                           select n.valor).Sum();
               cheque = (from n in listaPagamento
                         where n.tipopagamento == "CH"
                         && n.data == item.Value
                         select n.valor).Sum();
               crediario = (from n in listaPagamento
                            where n.tipopagamento == "CR"
                            && n.data == item.Value
                            select n.valor).Sum();
               ticket = (from n in listaPagamento
                         where n.tipopagamento == "TI"
                         && n.data == item.Value
                         select n.valor).Sum();
               cartaoCR = (from n in listaPagamento
                           where n.tipopagamento == "CA"
                           && n.data == item.Value
                           select n.valor).Sum();

                */
                #endregion

                decimal? dinheiroNota = (from n in listaPagamento
                                         where n.tipopagamento == "DH"
                                         && n.historico == "Nota Fiscal"
                                         && n.data == item.Value
                                         select (decimal?)n.valor).Sum();



                decimal? dinheiroNFe = (from n in Conexao.CriarEntidade().contnfsaida
                                        where n.dataemissao == item.Value && n.codigofilial == GlbVariaveis.glb_filial
                                        select (decimal?)n.totalNF).Sum();



                string meioPagamento = "Dinheiro";
                string tipoDoc = "1";

                dinheiro = 0;
                cheque = 0;
                crediario = 0;
                ticket = 0;
                cartaoDB = 0;
                cartaoCR = 0;
                dinheiroRec = 0;
                outros = 0;

                var pagamentoDiario = (from n in listaPagamento
                                       where n.data == item
                                       select n.data);



                dinheiro = (from n in listaPagamento
                            where n.tipopagamento == "DH" || n.tipopagamento == "XX"
                            && n.data == item.Value
                            select n.valor).Sum();



                dinheiroRec = (from n in listaPagamento
                               where n.tipopagamento == "DH"
                               && n.data == item.Value
                               && n.dpfinanceiro.Contains("Rec")
                               select n.valor).Sum();



                cheque = (from n in listaPagamento
                          where n.tipopagamento == "CH"
                          && n.data == item.Value
                          select n.valor).Sum();



                crediario = (from n in listaPagamento
                             where n.tipopagamento == "CR"
                             && n.data == item.Value
                             select n.valor).Sum();



                ticket = (from n in listaPagamento
                          where n.tipopagamento == "TI"
                          && n.data == item.Value
                          select n.valor).Sum();



                cartaoCR = (from n in listaPagamento
                            where n.tipopagamento == "CA"
                            && n.data == item.Value
                            select n.valor).Sum();


                
                outros = (from n in listaPagamento
                          where n.tipopagamento != "CA" && n.tipopagamento != "DH" && n.tipopagamento != "CH" && n.tipopagamento != "CR" && n.tipopagamento != "SI" && n.tipopagamento != "SU"
                            && n.data == item.Value
                            select n.valor).Sum();


                outrosQ = (from n in listaPagamento
                           where n.tipopagamento != "CA" && n.tipopagamento != "DH" && n.tipopagamento != "CH" && n.tipopagamento != "CR" && n.tipopagamento != "SI" && n.tipopagamento != "SU"
                                && n.data == item.Value
                              select n.valor).Count();


                // conteudo = item.tipopagamento+" Cupom Fiscal "+string.Format("{0:n2}",item.valor)+" "+string.Format("{0:dd/MM/yyyy}",item.data)+Environment.NewLine;
                // FuncoesECF.RelatorioGerencial("imprimir", conteudo);

                if (outros > 0 || outrosQ > 0)
                {
                    #region
                    meioPagamento = "Outros".PadRight(25, ' ').Substring(0, 25);

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "DH" || n.tipopagamento == "XX"
                                      select n);

                    meioPagamento = "Outros".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {
                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaOutros") == false)
                        {
                            meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                            if (itemPag.tipopagamento == "XX")
                            {
                                meioPagamento = "Outros XX".PadRight(25, '?').Substring(0, 25);
                            }

                        }

                        if (itemPag.ocorrencia == "9")
                        {
                            meioPagamento = "Outros".PadRight(25, '?').Substring(0, 25);
                        }

                    }


                    conteudo.AppendLine("A2" +
                    string.Format("{0:yyyyMMdd}", item) +
                    meioPagamento +
                    tipoDoc +
                    Funcoes.FormatarZerosEsquerda(dinheiro, 12, true));
                    #endregion
                }


                if (dinheiro > 0)
                {
                    #region
                    meioPagamento = "Dinheiro".PadRight(25, ' ').Substring(0, 25);

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "DH" || n.tipopagamento == "XX"
                                      select n);

                    meioPagamento = "Dinheiro".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {
                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaDH") == false)
                        {
                            meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                            if (itemPag.tipopagamento == "XX")
                            {
                                meioPagamento = "Dinheiro XX".PadRight(25, '?').Substring(0, 25);
                            }

                        }

                        if (itemPag.ocorrencia == "9")
                        {
                            meioPagamento = "Dinheiro".PadRight(25, '?').Substring(0, 25);
                        }

                    }


                    conteudo.AppendLine("A2" +
                    string.Format("{0:yyyyMMdd}", item) +
                    meioPagamento +
                    tipoDoc +
                    Funcoes.FormatarZerosEsquerda(dinheiro, 12, true));

                    #endregion
                }

                if (dinheiroRec > 0)
                {
                    #region
                    tipoDoc = "2";
                    meioPagamento = "Dinheiro".PadRight(25, ' ').Substring(0, 25);

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "DH"
                                      select n);


                    foreach (var itemPag in ListaPagDH)
                    {

                        meioPagamento = "Dinheiro".PadRight(25, ' ').Substring(0, 25);
                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaDH") == false)
                        {
                            meioPagamento = " XX" + meioPagamento.Trim().PadRight(25, '?');
                        }
                    }



                    conteudo.AppendLine("A2" +
                    string.Format("{0:yyyyMMdd}", item) +
                    meioPagamento +
                    tipoDoc +
                    Funcoes.FormatarZerosEsquerda(dinheiroRec, 12, true));
                    #endregion
                }

                //if (dinheiroNota > 0)
                //{
                //    meioPagamento = "Dinheiro Nota".PadRight(25, ' ').Substring(0, 25);
                //    tipoDoc = "2";
                //    conteudo.AppendLine("A2" +
                //                           string.Format("{0:yyyyMMdd}", item) +
                //                           meioPagamento +
                //                           tipoDoc +
                //                           Funcoes.FormatarZerosEsquerda(dinheiroNota.Value, 12, true));

                //}

                if (dinheiroNFe > 0)
                {
                    #region
                    string sqlsoma = "SELECT sum(total) from vendaarquivo where notafiscal is not null and codigofilial ='"+GlbVariaveis.glb_filial+"'";
                    decimal? dhNota = Conexao.CriarEntidade().ExecuteStoreQuery<decimal?>(sqlsoma).FirstOrDefault();


                    meioPagamento = "Dinheiro ".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "3";

                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(dinheiroNFe.Value + dhNota.Value + dinheiroNota.Value, 12, true));
                    #endregion
                }

                if (cheque > 0)
                {
                    #region
                    meioPagamento = "Cheque".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "1";

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "CH"
                                      select n);

                    meioPagamento = "Cheque".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {


                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaCH") == false)
                        {
                            meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                        }

                        if (itemPag.ocorrencia == "9")
                        {
                            tipoDoc = "9";
                            meioPagamento = "Cheque".PadRight(25, '?').Substring(0, 25);
                        }
                    }

                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(cheque, 12, true));

                    #endregion
                }

                if (crediario > 0)
                {
                    #region
                    meioPagamento = "Crediário".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "1";

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "CR"
                                      select n);
                    meioPagamento = "Crediário".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {

                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaCR") == false)
                        {
                            meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                        }

                        if (itemPag.ocorrencia == "9")
                        {
                            meioPagamento = "Crediário".PadRight(25, '?').Substring(0, 25);
                        }
                    }

                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(crediario, 12, true));

                    #endregion
                }

                if (cartaoDB > 0)
                {
                    #region
                    meioPagamento = "Cartao".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "1";

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "CA"
                                      select n);

                    meioPagamento = "Cartao".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {


                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaCA") == false)
                        {
                            meioPagamento = meioPagamento.Trim().PadRight(25, '?');
                        }

                        if (itemPag.ocorrencia == "9")
                        {
                            meioPagamento = "Cartao".PadRight(25, '?').Substring(0, 25);
                        }


                    }

                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(cartaoDB, 12, true));
                    #endregion
                }

                if (cartaoCR > 0)
                {
                    #region
                    meioPagamento = "Cartao".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "1";

                    var ListaPagDH = (from n in listaPagamento
                                      where n.tipopagamento == "CA"
                                      select n);

                    meioPagamento = "Cartao".PadRight(25, ' ').Substring(0, 25);
                    foreach (var itemPag in ListaPagDH)
                    {

                        var cripto = Funcoes.CriptografarMD5(itemPag.ecffabricacao + itemPag.coo + itemPag.ccf + itemPag.gnf + itemPag.ecfmodelo + itemPag.valor.ToString().Replace(",", ".") + itemPag.tipopagamento + string.Format("{0:yyyy-MM-dd}", itemPag.data) + itemPag.tipodoc);

                        if (cripto != itemPag.eaddados || FuncoesPAFECF.VerificarQtdRegistro("caixaCA") == false)
                        {
                            meioPagamento = "Cartao".PadRight(25, '?').Substring(0, 25);
                        }
                    }

                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(cartaoCR, 12, true));

                    #endregion
                }

                if (ticket > 0)
                {
                    #region
                    meioPagamento = "Ticket".PadRight(25, ' ').Substring(0, 25);
                    tipoDoc = "1";
                    conteudo.AppendLine("A2" +
                                         string.Format("{0:yyyyMMdd}", item) +
                                         meioPagamento +
                                         tipoDoc +
                                         Funcoes.FormatarZerosEsquerda(ticket, 12, true));
                    #endregion
                }
            }


            return conteudo;
        }
        #endregion

        #region P2

        public StringBuilder P2(string tabelaPreco)
        {
                LogSICEpdv.Registrarlog("P2("+tabelaPreco+")", "", "PAFArquivos.cs");
           
                siceEntities entidade = Conexao.CriarEntidade();
                entidade.CommandTimeout = 3600;

                var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

                if (!FuncoesPAFECF.VerificarQtdRegistro("produtos"))
                {
                    razao = razao.Trim().PadRight(50, '?');
                }

                StringBuilder conteudo = new StringBuilder();
                try
                {
                        #region
                        // conteudo = RegistroU1("");


                        // Registro substituido peloU1
                        /*conteudo.AppendLine("P1" +
                            Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                            Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                            Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                            razao);
                        */
                        var produtos = (from n in entidade.produtos
                                        where n.CodigoFilial == GlbVariaveis.glb_filial
                                        select new
                                        {
                                            n.codigo,
                                            n.descricao,
                                            n.unidade,
                                            n.unidembalagem,
                                            n.indicadorarredondamentotruncamento,
                                            n.indicadorproducao,
                                            n.tributacao,
                                            n.tipo,
                                            n.icms,
                                            n.precovenda,
                                            n.precoatacado,
                                            n.EADP2relacaomercadoria,
                                            n.STecf
                                        })
                                       .Concat(
                                       from n in entidade.produtosfilial
                                       where n.CodigoFilial == GlbVariaveis.glb_filial
                                       select new
                                       {
                                           n.codigo,
                                           n.descricao,
                                           n.unidade,
                                           n.unidembalagem,
                                           n.indicadorarredondamentotruncamento,
                                           n.indicadorproducao,
                                           n.tributacao,
                                           n.tipo,
                                           n.icms,
                                           n.precovenda,
                                           n.precoatacado,
                                           n.EADP2relacaomercadoria,
                                           n.STecf
                                       });

                        int nRegistros = 0;
                        if (tabelaPreco == "varejo")
                        {
                            produtos = from n in produtos
                                       where n.precovenda > 0
                                       select n;
                        }

                        if (tabelaPreco == "atacado")
                        {
                            produtos = from n in produtos
                                       where n.precoatacado > 0
                                       select n;
                        }

                        if (produtos.Count() > 0)
                        {
                            foreach (var item in produtos)
                            {
                                /*
                                 * I = Isenção
                                 * N = Não incidência
                                 * F = Substituição Tributária
                                 */

                                try
                                {
                                    decimal preco = item.precovenda;
                                    string unidade = item.unidade;

                                    var tributacao = SituacaoTributacao(Convert.ToInt16(item.icms), item.tributacao, item.tipo);

                                    if (tabelaPreco == "atacado")
                                    {
                                        preco = item.precoatacado;
                                        unidade = item.unidembalagem;

                                    }

                                    var cripto = Funcoes.CriptografarMD5(item.codigo.Trim() + item.descricao + item.icms.ToString().Replace(",", ".") + item.precovenda.ToString().Replace(",", ".") + item.precoatacado.ToString().Replace(",", ".") + unidade + item.indicadorarredondamentotruncamento + item.indicadorproducao + item.tributacao);
                                    if (cripto != item.EADP2relacaomercadoria && item.EADP2relacaomercadoria != null)
                                    {
                                        unidade = item.unidade.Trim().PadRight(6, '?');
                                    }
                                    conteudo.AppendLine("P2" +
                                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                                        item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +
                                        item.descricao.Trim().PadRight(50, ' ').Substring(0, 50) +
                                        unidade.PadRight(6, ' ').Substring(0, 6) +
                                        item.indicadorarredondamentotruncamento +
                                        item.indicadorproducao +
                                        tributacao +
                                        item.icms.ToString().Replace(",", "").PadRight(4, '0').Substring(0, 4) +
                                        Funcoes.FormatarZerosEsquerda(preco, 12, true));
                                    nRegistros++;
                                }
                                catch (Exception erro)
                                {
                                    MessageBox.Show(erro.Message);

                                }
                            }
                        }

                        return conteudo;

                        #endregion
                }
                catch (Exception erro)
                {
                    MessageBox.Show("Erro PAFArquivo function P2 -> "+erro.ToString());
                    return conteudo;
                }
                
        }
        #endregion

        #region E2
        public StringBuilder E2(string tipo)
        {
            LogSICEpdv.Registrarlog("E2(" + tipo + ")", "", "PAFArquivos.cs");
            // Antes da versao 01.08 do PAF criptografia do saldo em estoque
            //@tabelaProduto,'.EADE2mercadoriaEstoque=md5( concat(',@tabelaProduto,'.codigo,',@tabelaProduto,'.descricao,',@tabelaProduto,'.quantidade-( select sum(vendas.quantidade) from vendas where vendas.cancelado="N" and vendas.codigo=',@tabelaProduto,'.codigo ),',@tabelaProduto,'.dataultvenda) ),',
            string marcado = " ";
            if (tipo.ToLower() == "total")
            {
                string sql = "UPDATE produtos set marcado=''";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                sql = "UPDATE produtosfilial set marcado=''";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);

            }
            if (tipo == "parcial")
                marcado = "P";

            
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                entidade.CommandTimeout = 3600;

                string dataEstoque = string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
                string horaEstoque = string.Format("{0:hhmmss}", DateTime.Now.TimeOfDay);

                var estoque = (from n in entidade.produtos
                               where n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.marcado == marcado         
                               && n.situacao!="Inativo"
                               && n.EADE2mercadoriaEstoque != null
                               select new
                               {
                                   n.codigo,
                                   n.descricao,
                                   n.unidade,
                                   n.saldofinalestoque,
                                   n.data,
                                   n.datafinalestoque,
                                   n.horafinalestoque,
                                   n.EADE2mercadoriaEstoque,
                                   n.EADE1,
                                   n.dataultvenda,
                                   n.marcado,
                                   n.ecffabricacao,
                               });


                var itensEstoque = estoque.ToList();
          
                foreach (var itemE1 in estoque)
                {
                    var criptoE1 = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", itemE1.datafinalestoque) + itemE1.horafinalestoque.ToString() + itemE1.ecffabricacao);
                    if (criptoE1 != itemE1.EADE1)
                    {
                        //nrFabricacao = itensEstoque.First().ecffabricacao;
                        //modeloECF = ConfiguracoesECF.modeloECF.Trim().PadRight(20, '?'); ;
                        horaEstoque = string.Format("{0:hhmmss}", itemE1.horafinalestoque.Value);
                        dataEstoque = string.Format("{0:yyyyMMdd}", itemE1.datafinalestoque.Value);
                    }

                }
                var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

                if (!FuncoesPAFECF.VerificarQtdRegistro("produtos"))
                {
                    razao = razao.Trim().PadRight(50, '?');
                }

                StringBuilder conteudo = new StringBuilder();
                //Não existe mais
                /*
                conteudo.AppendLine("E1" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                    razao+
                    nrFabricacao.PadRight(20,' ').Substring(0,20)+
                    "1"+ //mfadicional
                       tipoECF.PadRight(7,' ').Substring(0,7)+
                        marcaECF.PadRight(20,' ').Substring(0,20)+
                        modeloECF.PadRight(20,' ').Substring(0,20)+
                        dataEstoque+
                        horaEstoque
                    );
                 */


                int nRegistros = 0;
                foreach (var item in estoque)
                {
                    //var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", item.datafinalestoque) + item.horafinalestoque.ToString() + item.ecffabricacao);
                   var unidade = item.unidade.Trim().PadRight(6, ' ').Substring(0, 6);
                   var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", item.datafinalestoque) + item.horafinalestoque.ToString() + item.ecffabricacao + item.codigo.Trim() + item.descricao.Trim() + item.unidade + item.saldofinalestoque.ToString().Replace(",","."));
                   if (cripto != item.EADE1)
                   {
                       unidade = item.unidade.Trim().PadRight(6, '?');
                   }

                 
                    //var cripto2 = Funcoes.CriptografarMD5(item.codigo.Trim() + item.descricao.Trim() + item.saldofinalestoque.ToString().Trim().Replace(",", ".") + string.Format("{0:yyyy-MM-dd}", item.datafinalestoque) + item.horafinalestoque.ToString() + item.ecffabricacao);
                    //if (cripto2 != item.EADE2mercadoriaEstoque)
                    //    unidade = item.unidade.Trim().PadRight(6, '?');


                    string posicaoEstoque = "+";
                    if (item.saldofinalestoque < 0)
                        posicaoEstoque = "-";

                    conteudo.AppendLine("E2" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                        item.codigo.Trim().PadRight(14, ' ').Substring(0, 14) +
                        item.descricao.Trim().PadRight(50, ' ').Substring(0, 50) +
                        unidade.PadRight(6, ' ').Substring(0, 6) +
                        posicaoEstoque +
                        Funcoes.FormatarZerosEsquerda(item.saldofinalestoque, 9, true).Replace("-", ""));
                    nRegistros++;
                    if (marcado == "P")
                    {
                        siceEntities entidadePR = Conexao.CriarEntidade();
                        var alterarMarcado = (from n in entidadePR.produtos
                                              where n.codigo == item.codigo
                                              select n).First();
                        alterarMarcado.marcado = " ";
                        entidadePR.SaveChanges();
                    }
                }

                return conteudo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region E3
        public StringBuilder E3()
        {

            LogSICEpdv.Registrarlog("E3()", "", "PAFArquivos.cs");

            StringBuilder conteudo = new StringBuilder();
            // conteudo = RegistroU1("produtos");

            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;


            string dataEstoque = string.Format("{0:yyyyMMdd}", DateTime.Now.Date);
            string horaEstoque = string.Format("{0:hhmmss}", DateTime.Now.TimeOfDay);


                var estoque = (from n in entidade.produtos
                               where n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.EADE2mercadoriaEstoque != null
                               select new
                               {
                                   n.codigo,
                                   n.descricao,
                                   n.unidade,
                                   n.saldofinalestoque,
                                   n.data,
                                   n.datafinalestoque,
                                   n.horafinalestoque,
                                   n.EADE2mercadoriaEstoque,
                                   n.EADE1,
                                   n.dataultvenda,
                                   n.marcado,
                                   n.ecffabricacao,
                               }).FirstOrDefault();



           if(estoque == null && GlbVariaveis.glb_filial != "00001")
           {

                estoque = (from n in entidade.produtosfilial
                               where n.CodigoFilial == GlbVariaveis.glb_filial
                               && n.EADE2mercadoriaEstoque != null
                               select new
                               {
                                   n.codigo,
                                   n.descricao,
                                   n.unidade,
                                   n.saldofinalestoque,
                                   n.data,
                                   n.datafinalestoque,
                                   n.horafinalestoque,
                                   n.EADE2mercadoriaEstoque,
                                   n.EADE1,
                                   n.dataultvenda,
                                   n.marcado,
                                   n.ecffabricacao,
                               }).FirstOrDefault();

            }


                var itensEstoque = estoque;

                string nrFabricacao = itensEstoque.ecffabricacao;// ConfiguracoesECF.nrFabricacaoECF;

                var dadosECF = (from n in Conexao.CriarEntidade().r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF && n.codigofilial == GlbVariaveis.glb_filial
                                select new { n.fabricacaoECF, n.modeloECF, n.tipoECF, n.marcaECF }).FirstOrDefault();
                if (FuncoesPAFECF.VerificarQtdRegistro("r01") == false)
                {
                    var ultimaECF = (from r in Conexao.CriarEntidade().r01 where r.codigofilial == GlbVariaveis.glb_filial select r.id).Max();
                    dadosECF = (from n in Conexao.CriarEntidade().r01
                                    where n.id == ultimaECF && n.codigofilial == GlbVariaveis.glb_filial
                                    select new { n.fabricacaoECF, n.modeloECF, n.tipoECF, n.marcaECF }).FirstOrDefault();

                    nrFabricacao = dadosECF.fabricacaoECF.ToString();
                }
               
                
                string tblProdutos = "produtos";
                if (GlbVariaveis.glb_filial != "00001")
                    tblProdutos = "produtosfilial";

                string sql = "SELECT eade3 FROM "+tblProdutos+" WHERE codigo = '" + itensEstoque.codigo + "' and codigofilial = '" + GlbVariaveis.glb_filial + "'";
                string eade3 = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();


            
            
            string modeloECF = dadosECF == null ? ConfiguracoesECF.modeloECF : dadosECF.modeloECF;
            string marcaECF = dadosECF == null ? ConfiguracoesECF.marcaECF : dadosECF.marcaECF;
            string tipoECF = dadosECF == null ? ConfiguracoesECF.tipoECF : dadosECF.tipoECF;

            horaEstoque = string.Format("{0:hhmmss}", itensEstoque.horafinalestoque.Value);
            dataEstoque = string.Format("{0:yyyyMMdd}", itensEstoque.datafinalestoque.Value);



            var unidade = itensEstoque.unidade.Trim().PadRight(6, ' ').Substring(0, 6);
            //antes var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", itensEstoque.datafinalestoque) + itensEstoque.horafinalestoque.ToString() + itensEstoque.ecffabricacao);
            var cripto = Funcoes.CriptografarMD5(string.Format("{0:yyyy-MM-dd}", itensEstoque.datafinalestoque) + itensEstoque.horafinalestoque.ToString() + itensEstoque.ecffabricacao + tipoECF + marcaECF + modeloECF);

                if (cripto != eade3)
                {
                    modeloECF = modeloECF.Trim().PadRight(20, '?');
                }
                                     
                conteudo.AppendLine("E3" +
                      nrFabricacao.PadRight(20, ' ').Substring(0, 20) +
                       " " + //mfadicional
                        tipoECF.PadRight(7, ' ').Substring(0, 7) +
                        marcaECF.PadRight(20, ' ').Substring(0, 20) +
                        modeloECF.PadRight(20, ' ').Substring(0, 20) +
                        dataEstoque +
                        horaEstoque);
            
            return conteudo;
        }
        #endregion

        #region D2
        public StringBuilder D2(DateTime dataInicial,DateTime dataFinal) // Dav Emitidos
        {
            LogSICEpdv.Registrarlog("D2(" + dataInicial + "," + dataFinal + ")", "", "PAFArquivos.cs");

            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;
            var dados = from n in entidade.contdav
                        where n.data >= dataInicial.Date && n.data <= dataFinal.Date && n.codigofilial == GlbVariaveis.glb_filial && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                        select new
                        {
                            numeroFabECF = n.ecffabricacao == null ? " " : n.ecffabricacao,
                            MFAdicional = " ",
                            //tipoECF = ConfiguracoesECF.tipoECF,
                            marcaECF = n.marca,
                            modeloECF = n.modelo,
                            COO = n.ncupomfiscal == null ? " " : n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal == null ? " " : n.ecfcontadorcupomfiscal,
                            DAVNumero = n.numeroDAVFilial,
                            data = n.data,
                            titulo = "ORÇAMENTO",
                            valor = n.valor,
                            COOVinculado = "",
                            ead = n.EADRegistroDAV,
                            nome = n.cliente,
                            cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor,
                            n.finalizada,
                            ecfNumero = n.numeroECF == null ? "001" : n.numeroECF,
                            n.contadorRGECF
                        };


            if (dados.Count() == 0)
            {
                StringBuilder vazio= new StringBuilder();
                return vazio;
            }

              try
                {
                    #region DAV Arquivo
                    StringBuilder conteudo = new StringBuilder();

                    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);
                    if (!FuncoesPAFECF.VerificarQtdRegistro("contdav"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                    if (!FuncoesPAFECF.VerificarQtdRegistro("vendadav"))
                    {
                        razao = razao.Trim().PadRight(50, '?');
                    }

                    // conteudo = RegistroU1("");
                    //Não existe mais
                    /*
                    conteudo.AppendLine("D1" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                        Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                        Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                        razao);
                    */

                    
                    
                    int nRegistrosD2 = 0;
                    foreach (var item in dados)
                    {

                        string sql = "SELECT IFNULL(tipoECF,'') FROM `contdav` WHERE `numeroDAVFilial` = '" + item.DAVNumero + "' AND codigofilial = '"+GlbVariaveis.glb_filial+"'; ";
                        string tipoECF = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                        sql = "SELECT IFNULL(mfAdicional,'') FROM `contdav` WHERE `numeroDAVFilial` = '" + item.DAVNumero + "' AND codigofilial = '" + GlbVariaveis.glb_filial + "'; ";
                        string mfAdicional = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                        //var cripto = Funcoes.CriptografarMD5(item.COO + item.DAVNumero.ToString() + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.valor.ToString().Replace(",", ".") + item.ecfNumero + item.contadorRGECF + item.nome + item.cnpjcpf);
                        //string mfAdicional = ConfiguracoesECF.mfAdicionalECF;
                        var cripto = Funcoes.CriptografarMD5(item.numeroFabECF + mfAdicional + tipoECF + item.marcaECF + item.modeloECF + item.contadorRGECF + item.DAVNumero + string.Format("{0:yyyy-MM-dd}", item.data) + item.valor.ToString().Replace(",", ".") + item.COO + item.ecfNumero + item.nome + item.cnpjcpf);
                        var numeroFabECF = item.numeroFabECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        var modeloECF = item.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);

                        string marcaECF = item.marcaECF == null ? "" : item.marcaECF;
                        tipoECF = tipoECF == null ? "" : tipoECF;
                        string ecfFabricacao = item.numeroFabECF  == null ? "" : item.numeroFabECF;
                       

                        if (ecfFabricacao == "" || ecfFabricacao == null)
                        {
                            marcaECF = " ";
                            tipoECF = " ";
                            modeloECF = " ";
                            mfAdicional = " ";
                        }


                        string contadorRG = item.contadorRGECF.Trim();
                        string numeroECF = item.ecfNumero;
                        string COO = item.COO;


                        //if (!string.IsNullOrEmpty(item.COO))
                        //{
                        //    modeloECF = " ";

                        //}
                        if (cripto != item.ead)
                        {

                            modeloECF = modeloECF.Trim().PadRight(20, '?');// "????????????????????";
                            numeroECF = item.ecfNumero;
                            contadorRG = item.contadorRGECF.Replace(" ", "");
                        }

                       
                        conteudo.AppendLine("D2" +
                        Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) + //02                             
                            ecfFabricacao.PadRight(20, ' ').Substring(0, 20) + //02                           
                            mfAdicional +
                            tipoECF.PadRight(7, ' ').Substring(0, 7) + //05                            
                            marcaECF.PadRight(20, ' ').Substring(0, 20) + //06                            
                            modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) + //??????????07                            
                            contadorRG.Trim().PadLeft(9, '0').Substring(0, 9) +
                            item.DAVNumero.ToString().PadLeft(13,'0').Substring(0,13) +
                            //Funcoes.FormatarZerosEsquerda(item.DAVNumero, 13, true)+ //+ "   " + // item.DAVNumero.ToString().Trim().PadRight(13, ' ').Substring(0, 13) +
                            string.Format("{0:yyyyMMdd}", item.data) +
                            item.titulo.Trim().PadRight(30, ' ').Substring(0, 30) +
                            Funcoes.FormatarZerosEsquerda(item.valor, 08, true) +
                            COO.PadRight(9, '0').Substring(0, 9) +
                            numeroECF.PadRight(3, '0').Substring(0, 3) +
                            item.nome.PadRight(40, ' ').Substring(0, 40) +
                            item.cnpjcpf.PadLeft(14, '0').Substring(0, 14)
                        );
                        nRegistrosD2++;
                    }


                    int nRegistrosD3 = 0;
                    foreach (var item in dados)
                    {
                        var itensDAV = from n in Conexao.CriarEntidade().vendadav
                                       where n.documento == item.DAVNumero
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select new
                                       {
                                           n.data,
                                           n.nrcontrole,
                                           n.codigo,
                                           n.produto,
                                           n.quantidade,
                                           n.unidade,
                                           n.preco,
                                           descontovalorTotal = (n.descontovalor * n.quantidade),
                                           n.acrescimototalitem,
                                           n.total,
                                           n.tipo,
                                           n.tributacao,
                                           n.icms,
                                           n.cancelado,
                                           n.documento,
                                           n.eaddados,
                                           n.Descontoperc,
                                           n.descontovalor,
                                           n.ccf,
                                           n.coo,
                                           n.ecffabricacao,
                                           n.horaalteracao,
                                           n.tipoalteracao
                                       };


                        foreach (var itens in itensDAV)
                        {

                            var cripto = Funcoes.CriptografarMD5(
                                itens.documento.ToString().Replace(",", ".") +
                                string.Format("{0:yyyy-MM-dd}", itens.data.Value) +
                                itens.nrcontrole.ToString().Replace(",", ".") + 
                                itens.codigo + 
                                itens.produto + 
                                itens.quantidade.ToString().Replace(",", ".") + 
                                itens.unidade + 
                                itens.preco.ToString().Replace(",", ".") + 
                                itens.descontovalor.ToString().Replace(",", ".") + 
                                itens.acrescimototalitem.ToString().Replace(",", ".") + 
                                itens.total.ToString().Replace(",", ".") + 
                                itens.tributacao.ToString() + 
                                itens.Descontoperc.ToString().Replace(",", ".") + 
                                itens.cancelado + 
                                itens.icms.ToString().Replace(",", ".") + 
                                itens.ccf + 
                                itens.coo + 
                                itens.ecffabricacao+
                                itens.horaalteracao+
                                itens.tipoalteracao);


                            var descricaoPrd = itens.produto;
                            if (cripto != itens.eaddados)
                            {
                                descricaoPrd = itens.produto.Trim().PadRight(100, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";

                            }
                            try
                            {
                                conteudo.AppendLine("D3" +
                                 //Funcoes.FormatarZerosEsquerda(item.DAVNumero, 13, false) +
                                 item.DAVNumero.ToString().PadLeft(13,'0').Substring(0,13) +
                                 string.Format("{0:yyyyMMdd}", itens.data) +
                                 itens.nrcontrole.ToString().PadLeft(3, '0').Substring(0, 3) +
                                 itens.codigo.PadRight(14, ' ').Substring(0, 14) +
                                 descricaoPrd.PadRight(100, ' ').Substring(0, 100) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
                                 itens.unidade.PadRight(3, ' ').Substring(0, 3) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalorTotal), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
                                 Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
                                 SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
                                 Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
                                 itens.cancelado +
                                 "3" +
                                 "2");
                                nRegistrosD3++;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message);
                            }
                        }
                    }
                    //Não existe mais
                    /*
                    conteudo.AppendLine("D9" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    string.Format("{0:000000}", nRegistrosD2) +
                    string.Format("{0:000000}", nRegistrosD3)
                    );
                    */
                    return conteudo;

                    #endregion
                }
                catch (Exception erro)
                {
                    throw new Exception(erro.Message);
                }                     
        }
        #endregion

        #region D4
        public StringBuilder D4(DateTime dataInicial, DateTime dataFinal) // Log de Alteracao de itens do DAV
        {

            LogSICEpdv.Registrarlog("D4(" + dataInicial + "," + dataFinal + ")", "", "PAFArquivos.cs");
 
            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;
            var dados = from n in entidade.contdav
                        where n.data >= dataInicial.Date && n.data <= dataFinal.Date && n.codigofilial == GlbVariaveis.glb_filial && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                        select new
                        {
                            numeroFabECF = n.ecffabricacao,
                            MFAdicional = " ",
                            tipoECF = " ",
                            marcaECF = n.marca,
                            modeloECF = n.modelo,
                            COO = n.ncupomfiscal == null ? " " : n.ncupomfiscal, // O número do cupom fiscal COO
                            CCF = n.ecfcontadorcupomfiscal == null ? " " : n.ecfcontadorcupomfiscal,
                            DAVNumero = n.numeroDAVFilial,
                            data = n.data,
                            titulo = "ORÇAMENTO",
                            valor = n.valor,
                            COOVinculado = "",
                            ead = n.EADRegistroDAV,
                            nome = n.cliente,
                            cnpjcpf = n.ecfCPFCNPJconsumidor == null ? "" : n.ecfCPFCNPJconsumidor,
                            n.finalizada,
                            ecfNumero = n.numeroECF == null ? "001" : n.numeroECF,
                            n.contadorRGECF
                        };


            if (dados.Count() == 0)
            {
                StringBuilder vz = new StringBuilder();
                return vz;
            }

            try
            {

                
                StringBuilder conteudo = new StringBuilder();

                // conteudo = RegistroU1("vendadav");


                int nRegistrosD3 = 0;
                foreach (var item in dados)
                {

                    var itensDAV = from n in Conexao.CriarEntidade().vendadav
                                   where n.documento == item.DAVNumero
                                   && n.codigofilial == GlbVariaveis.glb_filial
                                   && (n.tipoalteracao == "I" || n.tipoalteracao == "A" || n.tipoalteracao == "E")
                                   select new
                                   {
                                       n.data,
                                       n.nrcontrole,
                                       n.codigo,
                                       n.produto,
                                       n.quantidade,
                                       n.unidade,
                                       n.preco,
                                       descontovalorTotal = (n.descontovalor * n.quantidade),
                                       n.acrescimototalitem,
                                       n.total,
                                       n.tipo,
                                       n.tributacao,
                                       n.icms,
                                       n.cancelado,
                                       n.documento,
                                       n.eaddados,
                                       n.Descontoperc,
                                       n.descontovalor,
                                       n.ccf,
                                       n.coo,
                                       n.ecffabricacao,
                                       n.dataalteracao,
                                       n.horaalteracao,
                                       n.tipoalteracao
                                   };


                    foreach (var itens in itensDAV)
                    {

                        var cripto = Funcoes.CriptografarMD5(
                            itens.documento.ToString().Replace(",", ".") +
                            string.Format("{0:yyyy-MM-dd}", itens.data.Value) + 
                            itens.nrcontrole.ToString().Replace(",", ".") + 
                            itens.codigo + 
                            itens.produto + 
                            itens.quantidade.ToString().Replace(",", ".") + 
                            itens.unidade + 
                            itens.preco.ToString().Replace(",", ".") + 
                            itens.descontovalor.ToString().Replace(",", ".") + 
                            itens.acrescimototalitem.ToString().Replace(",", ".") + 
                            itens.total.ToString().Replace(",", ".") + 
                            itens.tributacao.ToString() + 
                            itens.Descontoperc.ToString().Replace(",", ".") + 
                            itens.cancelado + 
                            itens.icms.ToString().Replace(",", ".") + 
                            itens.ccf + 
                            itens.coo + 
                            itens.ecffabricacao+
                            itens.horaalteracao+
                            itens.tipoalteracao);


                        var descricaoPrd = itens.produto;
                        if (cripto != itens.eaddados)
                        {
                            descricaoPrd = itens.produto.Trim().PadRight(100, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";
                        }
                        try
                        {
                            conteudo.AppendLine("D4" +
                             Funcoes.FormatarZerosEsquerda(itens.documento, 13, false) +
                             string.Format("{0:yyyyMMdd}", itens.dataalteracao) + //DataAlteracao ??
                             string.Format("{0:hhmmss}", itens.horaalteracao) + // horaalteracao ??                                 
                             itens.codigo.PadRight(14, ' ').Substring(0, 14) +
                             descricaoPrd.PadRight(100, ' ').Substring(0, 100) +
                             Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.quantidade), 07, true) +
                             itens.unidade.PadRight(3, ' ').Substring(0, 3) +
                             Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.preco), 08, true) +
                             Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.descontovalor), 08, true) +
                             Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.acrescimototalitem), 08, true) +
                             Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(itens.total), 14, true) +
                             SituacaoTributacao(itens.icms, itens.tributacao, itens.tipo) +
                             Funcoes.FormatarZerosEsquerda(itens.icms, 4, true) +
                             itens.cancelado +
                             "3" +
                             "2" +
                             itens.tipoalteracao);
                            nRegistrosD3++;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }

                return conteudo;

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        
    }
        #endregion

        #region Arquivos R
        public StringBuilder GerarR(bool criarTabelaVenda, DateTime dataInicial, DateTime dataFinal, string numerodoECF, string nomeArquivoDestino,bool fechamentoZ=true, bool DIEF = false)
        {
           

            // Criando o arquivo temporário das vendas
            if (criarTabelaVenda)
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial.Date, dataFinal.Date, GlbVariaveis.glb_filial, false);
                Funcoes.CriarTabelaTmp("caixa", dataInicial.Date, dataFinal.Date, GlbVariaveis.glb_filial, false);
            }

            Funcoes.ProcedureAjuste("AjustarCamposNulos");
            

            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;
            StringBuilder conteudo = new StringBuilder();
            #region R01

            string modeloECF = " ";
            string fabricacaoECF = " ";
            string mfAdicionalECF = " ";
            try
            {
                /*
                var dadosR01 = (from n in entidade.r01                                
                                orderby n.data descending

                                select n).ToList().Take(100).Distinct();

                if (dadosR01.Count() == 0)
                {
                    dadosR01 = (from n in entidade.r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                orderby n.data descending
                                select n).Take(1);
                }
                */

                var dadosR01 = (from n in entidade.r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                orderby n.data descending
                                select n).ToList().Take(1);

                if (dadosR01.Count() == 0)
                {
                    throw new Exception("Não foi possível localizar a ECF de numero.: "+ConfiguracoesECF.nrFabricacaoECF+" na tabela R01");
                }

                if (fechamentoZ)
                {
                    dadosR01 = (from n in entidade.r01
                                where n.numeroECF == numerodoECF
                                && n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                select n).ToList().Take(1);

                    if (dadosR01.Count() == 0)
                    {
                        dadosR01 = (from n in entidade.r01
                                    where n.numeroECF == numerodoECF
                                    && n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                    select n).ToList().Take(1);
                    }
                }

                modeloECF = dadosR01.First().modeloECF;
                fabricacaoECF = dadosR01.First().fabricacaoECF ?? " ";
                mfAdicionalECF = ConfiguracoesECF.mfAdicionalECF; // dadosR01.First().MFAdicional ?? " ";


                foreach (var r1 in dadosR01)
                {
                    /// Criar uma List com os dados dos ECF's para serem usados a seguir
                    /// 
                 
                    var ECFModelo = r1.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                    var ECFModeloR1 = r1.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                   
                    if (r1.EADdados != Funcoes.CriptografarMD5(
                        r1.fabricacaoECF + 
                        r1.cnpj + 
                        r1.cnpjdesenvolvedora + 
                        r1.aplicativo + 
                        r1.md5+
                        r1.MFAdicional+
                        r1.tipoECF+
                        r1.modeloECF+
                        r1.versaoSB+
                        String.Format("{0:yyyy-MM-dd}", r1.datainstalacaoSB)+
                        r1.horainstalacaoSB+
                        r1.numeroECF+
                        r1.inscricao+
                        r1.inscricaodesenvolvedora+
                        r1.inscricaomunicipaldesenvolvedora+
                        r1.razaosocialdesenvolvedora+
                        r1.versao+
                        r1.versaoERPAF))
                    {
                        ECFModelo = r1.modeloECF.Trim().PadRight(20, '?'); //"????????????????????";
                        ECFModeloR1 = r1.modeloECF.Trim().PadRight(20, '?');
                    }

                    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

                    string razaosocialdesenvolvedora = r1.razaosocialdesenvolvedora.Trim().PadRight(40, ' ').Substring(0, 40);
                    if (!FuncoesPAFECF.VerificarQtdRegistro("vendaarquivo"))
                    {                        
                        ECFModelo = r1.modeloECF.Trim().PadRight(20, '?');
                    }

                    //aqui Implementar r01

                    conteudo.AppendLine("R01" + //01
                            r1.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) + //02
                            r1.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) + //03
                            r1.tipoECF.Trim().PadRight(7, ' ').Substring(0, 7) + //05
                            r1.marcaECF.Trim().PadRight(20, ' ').Substring(0, 20) + //06
                            ECFModeloR1 + //07
                            r1.versaoSB.Trim().PadRight(10, ' ').Substring(0, 10) + //08
                            String.Format("{0:yyyyMMdd}", r1.datainstalacaoSB) +  // dataInstalacaoSB+
                            String.Format("{0:hhmmss}", r1.horainstalacaoSB) + // horaInstalacaoSB+
                            r1.numeroECF.Trim().PadRight(3, ' ').Substring(0, 3) +
                            r1.cnpj.Trim().PadRight(14, '0').Substring(0, 14) + //12
                            r1.inscricao.Trim().PadLeft(14, ' ').Substring(0, 14) + //13
                            r1.cnpjdesenvolvedora.Trim().PadRight(14, '0').Substring(0, 14) + //14
                            r1.inscricaodesenvolvedora.Trim().PadLeft(14, '0').Substring(0, 14) + //15
                            r1.inscricaomunicipaldesenvolvedora.Trim().PadLeft(14, '0').Substring(0, 14) + //16
                            razaosocialdesenvolvedora + //17
                            r1.aplicativo.Trim().PadRight(40, ' ').Substring(0, 40) + //18
                            r1.versao.Trim().PadRight(10, ' ').Substring(0, 10) + //19
                            r1.md5.Trim().PadRight(32, ' ').Substring(0, 32) + //20
                            String.Format("{0:yyyyMMdd}", dataInicial) +
                            String.Format("{0:yyyyMMdd}", dataFinal) +
                            r1.versaoERPAF.Trim().PadRight(4, ' ').Substring(0, 4));
                };
            }
            catch (Exception ex)
            {
                throw new Exception("R01 - " + ex.Message);
            }
            #endregion

            //if (DIEF == false)
            //{
                #region R02
                try
                {
                    var dadosR02 = (from n in entidade.r02
                                   where n.data >= dataInicial && n.data <= dataFinal
                                   && n.numeroECF == numerodoECF && n.codigofilial == GlbVariaveis.glb_filial
                                   select n).ToList();

                    foreach (var r2 in dadosR02)
                    {
                    
                        var ECFModeloR2 = r2.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        if (r2.EADdados != Funcoes.CriptografarMD5(
                            r2.fabricacaoECF +
                            r2.crz +
                            r2.coo +
                            r2.cro +
                            string.Format("{0:yyyy-MM-dd}", r2.data) +
                            string.Format("{0:yyyy-MM-dd}", r2.dataemissaoreducaoz.Value) +
                            r2.horaemissaoreducaoz +
                            r2.vendabrutadiaria.ToString().Replace(",", ".") +
                            r2.modeloECF +
                            r2.MFadicional +
                            String.Format("{0:yyyy-MM-dd}", r2.datamovimento))
                            )
                        {
                            ECFModeloR2 = r2.modeloECF.Trim().PadRight(20, '?'); //ECFModeloR2 =  "????????????????????";
                        }

                        conteudo.AppendLine("R02" +
                                r2.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                                r2.MFadicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                                ECFModeloR2 +
                                r2.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, ' ').Substring(0, 2) +
                                r2.crz.Trim().PadRight(6, ' ').Substring(0, 6) +
                                r2.coo.Trim().PadRight(9, ' ').Substring(0, 9) +
                                r2.cro.Trim().PadRight(6, ' ').Substring(0, 6) + //08 Contador reinicio de operacao
                                String.Format("{0:yyyyMMdd}", r2.datamovimento) +
                                String.Format("{0:yyyyMMdd}", r2.dataemissaoreducaoz) +
                                String.Format("{0:hhmmss}", r2.horaemissaoreducaoz).Replace(":", "") +
                                Funcoes.FormatarZerosEsquerda(r2.vendabrutadiaria, 14, true) +
                                "N");
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception("R02 - " + ex.Message);
                }
                #endregion
           // }

            //if (DIEF == false)
            //{
                #region R03
                try
                {
                    var dadosR03 = from n in entidade.r03
                                   where n.data >= dataInicial && n.data <= dataFinal
                                   && n.numeroECF == numerodoECF && n.codigofilial == GlbVariaveis.glb_filial
                                   select n;

                    foreach (var r3 in dadosR03)
                    {
                        var ECFmodelo = r3.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                        var cripto = Funcoes.CriptografarMD5(r3.fabricacaoECF + r3.CRZ + r3.totalizadorParcial + r3.valoracumulado.ToString().Replace(",", ".") + r3.numeroUsuarioSubstituicaoECF + r3.modeloECF + r3.MFAdicional);
                        if (cripto != r3.EADdados)
                            ECFmodelo = r3.modeloECF.Trim().PadRight(20, '?');

                        conteudo.AppendLine("R03" +
                              r3.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                              r3.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                              ECFmodelo +
                              r3.numeroUsuarioSubstituicaoECF.Trim().PadRight(2, ' ').Substring(0, 2) +
                              r3.CRZ.Trim().PadRight(6, ' ').Substring(0, 6) +
                              r3.totalizadorParcial.Trim().PadRight(7, ' ').Substring(0, 7) +
                              Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(r3.valoracumulado), 13, true));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("R03 - " + ex.Message);
                }
            #endregion
            //}

            if (DIEF == true)
            {
                #region R04
                try
                {


                    decimal ValorLiquido = (from n in entidade.r03
                                            where n.totalizadorParcial != "OPNF"
                                            && n.totalizadorParcial != "AT"
                                            && !n.totalizadorParcial.StartsWith("Can")
                                            && !n.totalizadorParcial.StartsWith("D")
                                            && n.valoracumulado > 0
                                            && n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                            && n.fabricacaoECF == fabricacaoECF
                                            select n.valoracumulado.Value).Sum();

                    decimal ValorBruto = (from n in entidade.r03
                                          where n.totalizadorParcial != "OPNF"
                                          && n.totalizadorParcial != "AT"
                                          //&& !n.totalizadorParcial.StartsWith("Can")
                                          //&& !n.totalizadorParcial.StartsWith("D")
                                          && n.valoracumulado > 0
                                          && n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                          && n.fabricacaoECF == fabricacaoECF
                                          select n.valoracumulado.Value).Sum();





                    var obterDadosR04 = from d in entidade.contdocs
                                        where d.data >= dataInicial && d.data <= dataFinal
                                        && d.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                        && d.dpfinanceiro == "Venda"
                                        && d.CodigoFilial == GlbVariaveis.glb_filial
                                        //&& d.estornado == "N"
                                        && d.total > 0
                                        select new
                                        {
                                            cliente = d.ecfConsumidor == null ? " " : d.ecfConsumidor,
                                            cpfcnpj = d.ecfCPFCNPJconsumidor == null ? " " : d.ecfCPFCNPJconsumidor,
                                            estornado = d.estornado,
                                            ecfcontadorcupomfiscal = d.ecfcontadorcupomfiscal == null ? " " : d.ecfcontadorcupomfiscal,
                                            ncupomfiscal = d.ncupomfiscal == null ? " " : d.ncupomfiscal,
                                            ccf = d.ecfcontadorcupomfiscal == null ? " " : d.ecfcontadorcupomfiscal,
                                            data = d.data,
                                            ecftotalliquido = d.ecftotalliquido,
                                            ecftotalbruto = (d.Totalbruto),
                                            desconto = d.desconto,
                                            numeroECF = d.ecfnumero == null ? " " : d.ecfnumero,
                                            encargos = d.encargos,
                                            ecffabricao = d.ecffabricacao,
                                            mfadicional = d.ecfMFadicional,
                                            usuarioSubECF = "01",
                                            modeloECF = d.ecfmodelo,
                                            ecfmodelo = d.ecfmodelo,
                                            valorCancelado = d.ecfValorCancelamentos,
                                            d.ecffabricacao,
                                            d.contadornaofiscalGNF,
                                            d.contadordebitocreditoCDC,
                                            d.COOGNF,
                                            d.tipopagamento,
                                            d.EADr06
                                        };

                    var dadosR04 = (from n in obterDadosR04.ToList() where n.estornado == "N" select n).ToList();



                    

                    decimal Diferencaliquida = (ValorLiquido - dadosR04.Sum(r => r.ecftotalliquido));
                    decimal DiferencaBruta = (ValorBruto - (obterDadosR04.Sum(r => r.ecftotalbruto).Value + obterDadosR04.Sum(r => r.valorCancelado)));
                    
                    decimal DiferencaDesconto = (obterDadosR04.Sum(r => r.desconto).Value - ValorBruto);

                    //MessageBox.Show("valor Total "+(obterDadosR04.Sum(r => r.ecftotalbruto) + obterDadosR04.Sum(r => r.valorCancelado)).ToString()+
                        //" - Valor Bruto "+ ValorBruto + " diferenca "+ DiferencaBruta.ToString());

                    //MessageBox.Show("valor bruto " + (obterDadosR04.Sum(r => r.ecftotalbruto)).ToString() +
                        //" cancelamento " + obterDadosR04.Sum(r => r.valorCancelado).ToString());



                    if (obterDadosR04 != null)
                    {
                        decimal valorLiquido = 0;
                        decimal valorBruto = 0;

                        foreach (var item in obterDadosR04)
                        {


                            string indicadorDesconto = " ";

                            if (item.desconto > 0)
                                indicadorDesconto = "V";
                            string indicadorAcrescimo = " ";
                            if (item.encargos > 0)
                                indicadorAcrescimo = "V";
                            string ordemAcrescimoDesconto = " ";

                            if (item.desconto > 0)
                                ordemAcrescimoDesconto = "D";
                            if (item.encargos > 0)
                                ordemAcrescimoDesconto = "A";



                            if (item.estornado == "N")
                                valorBruto = item.ecftotalbruto.Value + item.valorCancelado;
                            else
                                valorBruto = item.valorCancelado;

                            


                            if (Diferencaliquida != 0 && item.estornado =="N")
                            {
                                if (Diferencaliquida > 0)
                                {
                                    valorLiquido = item.ecftotalliquido + Diferencaliquida;
                                    //valorBruto = valorBruto + Diferencaliquida;
                                }
                                else
                                {
                                    valorLiquido = item.ecftotalliquido - Diferencaliquida;
                                    valorBruto = valorBruto - Diferencaliquida;
                                }

                                Diferencaliquida = 0;
                            }
                            else
                            {
                                valorLiquido = item.ecftotalliquido;
                                if (item.estornado == "S")
                                    valorLiquido = 0;
                            }


                            var ECFModelo = item.ecfmodelo.Trim().PadRight(20, ' ').Substring(0, 20); // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                            var cripto = Funcoes.CriptografarMD5(
                                item.ecffabricacao +
                                item.ncupomfiscal +
                                item.contadornaofiscalGNF +
                                item.contadordebitocreditoCDC +
                                string.Format("{0:yyyy-MM-dd}",
                                item.data.Value) +
                                item.COOGNF +
                                item.tipopagamento +
                                item.ecfcontadorcupomfiscal +
                                //item.ecftotalliquido.ToString().Replace(",", ".") + 
                                valorLiquido.ToString().Replace(",", ".") +
                                item.estornado +
                                item.mfadicional +
                                item.ecfmodelo +
                                item.ccf +
                                //item.ecftotalbruto.ToString().Replace(",",".")+
                                valorBruto.ToString().Replace(",", ".") +
                                item.desconto.ToString().Replace(",", ".") +
                                item.encargos.ToString().Replace(",", ".") +
                                item.cliente +
                                item.cpfcnpj);

                            if (cripto != item.EADr06)
                            {
                                ECFModelo = item.ecfmodelo.Trim().PadRight(20, '?'); // "????????????????????";
                            }


                            conteudo.AppendLine("R04" +
                               //(from n in decf where n.ecfNumero==item.numeroECF select n.serieFabricacao).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                               //(from n in decf where n.ecfNumero==item.numeroECF select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                               //(from n in decf where n.ecfNumero==item.numeroECF select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                               //(from n in decf where n.ecfNumero==item.numeroECF select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +

                               item.ecffabricao.Trim().PadRight(20, ' ').Substring(0, 20) +
                               item.mfadicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                               ECFModelo.Trim().PadRight(20, ' ').Substring(0, 20) +
                               //item.usuarioSubECF.PadRight(2, ' ').Substring(0, 2) +
                               "01" +
                                item.ccf.Trim().PadRight(9, '0').Substring(0, 9) +
                                item.ncupomfiscal.Trim().PadRight(9, '0').Substring(0, 9) +
                                string.Format("{0:yyyyMMdd}", item.data) +
                                Funcoes.FormatarZerosEsquerda(valorBruto, 14, true) +
                                Funcoes.FormatarZerosEsquerda(Convert.ToDecimal(item.desconto), 13, true) +
                                indicadorDesconto + //10
                                Funcoes.FormatarZerosEsquerda(item.encargos, 13, true) +
                                indicadorAcrescimo +
                                Funcoes.FormatarZerosEsquerda(valorLiquido, 14, true) +
                                item.estornado +
                                Funcoes.FormatarZerosEsquerda(item.valorCancelado, 13, true) +
                                ordemAcrescimoDesconto +
                                item.cliente.PadRight(40, ' ').Substring(0, 40) +
                                item.cpfcnpj.PadLeft(14, '0').Substring(0, 14)
                                );
                        }
                    }
                } // != null
                catch (Exception ex)
                {
                    throw new Exception("R04 - " + ex.Message);
                }
                #endregion
            }

            #region R05
            try
            {
                var obterdadosR05 = from n in entidade.r05
                                    where n.data >= dataInicial && n.data <= dataFinal
                                    && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF                                 
                                    orderby n.documento
                                    select new
                                    {
                                        n.icms,
                                        n.tributacao,
                                        n.cancelado,
                                        n.ecfmodelo,
                                        n.documento,
                                        n.eaddados,
                                        ecffabricacao = n.ecffabricacao ?? " ",
                                        n.ecfcontadorcupomfiscal,
                                        n.ecfMFadicional,
                                        n.codigo,
                                        n.produto,
                                        n.ncupomfiscal,
                                        n.unidade,
                                        n.descontovalor,
                                        n.quantidade,
                                        n.nrcontrole,
                                        n.indicadorarredondamentotruncamento,
                                        n.indicadorproducao,
                                        n.preco,
                                        n.total,
                                        n.Descontoperc,
                                        n.data,
                                        n.acrescimototalitem,
                                        n.coo,
                                        n.ccf,
                                        n.estornado,
                                        n.canceladoECF
                                    };

                var dadosR05 = obterdadosR05.ToList();


                foreach (var item in dadosR05)
                {

                    var indexTotalizador = FuncoesFiscais.IndiceTotalizador(item.icms.ToString().Replace("0", ""), item.cancelado == "S" ? true : false); //?? aliquotasECF.IndexOf(item.icms.ToString().Replace(",", ""));                    


                    string totalizador = "0" + indexTotalizador.ToString().Trim() + "T" + item.icms.ToString().Replace(",", "") + "00";

                    if (item.icms == 0 || item.tributacao == "40")
                        totalizador = "I1     "; // Isenção
                    if (item.tributacao == "10" || item.tributacao == "30" || item.tributacao == "60" || item.tributacao=="70")
                        totalizador = "F1     "; // Substituição Tributária
                    if (item.tributacao == "80")
                        totalizador = "N1     "; // Não Incidência

                    if (item.canceladoECF == "S" && item.icms>0)
                    {
                        totalizador = "1Can-T  ";
                    }

                    var modeloECFR05 = item.ecfmodelo.Trim().PadRight(20, ' ').Substring(0, 20);

                    var cripto = Funcoes.CriptografarMD5(
                        item.documento.ToString().Replace(",", ".") + 
                        string.Format("{0:yyyy-MM-dd}", item.data.Value) + 
                        item.nrcontrole.ToString().Replace(",", ".") + 
                        item.codigo + 
                        item.produto + 
                        (string.Format("{0:N5}", item.quantidade).ToString().Replace(",", ".")) + 
                        item.unidade + 
                        item.preco.ToString().Replace(",", ".") + 
                        (string.Format("{0:N2}", item.descontovalor).ToString().Replace(",", ".")) + 
                        (string.Format("{0:N2}", item.acrescimototalitem).ToString().Replace(",", ".")) + 
                        item.total.ToString().Replace(",", ".") + 
                        item.tributacao.ToString() + 
                        (string.Format("{0:N2}", item.Descontoperc).ToString().Replace(",", ".")) + 
                        item.cancelado + item.icms.ToString().Replace(",", ".") + 
                        item.ccf + 
                        item.coo + 
                        item.ecffabricacao);

                    if (cripto != item.eaddados)
                    {
                        //  MessageBox.Show((string.Format("{0:N2}", itens.descontovalor).ToString().Replace(",", ".")));
                        modeloECFR05 = item.ecfmodelo.Trim().PadRight(20, '?'); // "????????????????????????????????????????????????????????????????????????????????????????????????????";
                    }

                    string valorCalado = "0";
                    decimal valor = (item.preco.Value - item.acrescimototalitem + item.descontovalor);
                    if (valor < 0)

                        valorCalado = Funcoes.FormatarZerosEsquerda(valor, 7, true);
                    else
                        valorCalado = Funcoes.FormatarZerosEsquerda(valor, 8, true);


                conteudo.AppendLine("R05" + //01
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.serieFabricacao).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +

                        item.ecffabricacao.Trim().PadRight(20, ' ').Substring(0, 20) +
                        ConfiguracoesECF.mfAdicionalECF.PadRight(1, ' ').Substring(0, 1) +
                        modeloECFR05 +
                        "01" +
                        item.ncupomfiscal.PadRight(9, ' ').Substring(0, 9) + //06
                        item.ecfcontadorcupomfiscal.Trim().PadRight(9, '0').Substring(0, 9) + //07
                        item.nrcontrole.ToString().PadLeft(3, '0').Substring(0, 3) + //08
                        item.codigo.PadRight(14, ' ').Substring(0, 14) + //09
                        item.produto.PadRight(100, ' ').Substring(0, 100) + //10
                        //string.Format("{0:N5}", item.quantidade).PadRight(7, '0').Substring(0, 7) + //11
                        Funcoes.FormatarZerosEsquerda(item.quantidade, 7, true) +
                        item.unidade.PadRight(3, ' ').Substring(0, 3) + //12
                        valorCalado +                                                //string.Format("{0:N2}", item.preco).PadRight(8, '0').Substring(0, 8) + //13
                        Funcoes.FormatarZerosEsquerda(item.descontovalor, 8, true) + //14
                        Funcoes.FormatarZerosEsquerda(item.acrescimototalitem, 8, true) + //15
                        Funcoes.FormatarZerosEsquerda(item.total, 14, true) + //16
                        totalizador.PadRight(7, ' ').Substring(0, 7) + // 17
                        item.canceladoECF + //18
                        Funcoes.FormatarZerosEsquerda(0, 7, true) + //19
                        Funcoes.FormatarZerosEsquerda(0, 13, true) + //20
                        Funcoes.FormatarZerosEsquerda(0, 13, true) + // 21 Cancelamento de Acréscimo
                        item.indicadorarredondamentotruncamento + //22
                        item.indicadorproducao + //23
                        "2" + //24
                        "3"); //25

                }
            }
            catch (Exception ex)
            {
                throw new Exception("R05 - " + ex.Message);
            }
            #endregion

            if (DIEF == false)
            {
                #region R06
                // Suprimento e Saldo Inicial

                // Tabelas por ordem para testar no PAF os ?? (contrelatoriogerencial -->> ecffabricao ), movdespesas -->  ,
                try
                {
                    try
                    {
                        var dadosR06Saldos = (from n in entidade.caixatmp
                                              where (n.tipopagamento == "SI" || n.tipopagamento == "SU")
                                              && n.valor > 0
                                              && n.historico.Substring(2, 3) == numerodoECF
                                              && n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                              && n.CodigoFilial == GlbVariaveis.glb_filial
                                              select new { n.historico, n.data, n.horaabertura, n.coo }).Concat(
                                              (from n in entidade.caixa
                                               where (n.tipopagamento == "SI" || n.tipopagamento == "SU")
                                               && n.valor > 0
                                               && n.historico.Substring(2, 3) == numerodoECF
                                               && n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                               && n.CodigoFilial == GlbVariaveis.glb_filial
                                               select new { n.historico, n.data, n.horaabertura, n.coo })
                                              );

                        var dadosSaldo = dadosR06Saldos.ToList();




                        foreach (var item in dadosSaldo)
                        {
                            #region
                            conteudo.AppendLine("R06" + //01
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2,3) select n.serieFabricacao).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2, 3) select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2, 3) select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2, 3) select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +                        

                                fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                                ConfiguracoesECF.mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                                modeloECF.Trim().PadRight(20, ' ').Substring(0, 20) +
                                "01" +
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2, 3) select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.historico.Substring(2, 3) select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +                                            

                                // Tem 000 antes do historico por que na versao anterior era necessario apenas 6 caracteres

                                //"000"+item .historico.PadRight(9, '0').Substring(15, 9) + // COO 09
                                item.coo.PadLeft(9, '0').Substring(0, 9) +
                                item.historico.PadRight(6, '0').Substring(7, 6) + //GNF 07                    
                                "000000" + // 08 Contador Relatorio Gerencial
                                "0000" + // 09
                                "CN" + //10
                                string.Format("{0:yyyyMMdd}", item.data) +
                                String.Format("{0:hhmmss}", item.horaabertura).Replace(":", ""));
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        //MessageBox.Show(ex.InnerException.ToString());
                        throw new Exception("R06 - caixa - " + ex.Message);
                    }


                    try
                    {
                        // Sangria
                        var dadosR06 = from n in entidade.movdespesas
                                       where n.data >= dataInicial && n.data <= dataFinal
                                       && n.ecfnumero != "" && n.ecfnumero != null
                                       && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                       && n.sangria == "S"
                                       && n.codigofilial == GlbVariaveis.glb_filial
                                       select new
                                       {
                                           n.data,
                                           contadornaofiscalGNF = n.contadornaofiscalGNF ?? "",
                                           EADDados = n.EADDados == null ? "" : n.EADDados,
                                           ncupomfiscalCOO = n.ncupomfiscalCOO ?? "",
                                           n.hora,
                                           ecfcontadorcupomfiscal = n.ecfcontadorcupomfiscal ?? "",
                                           tipopgamento = n.tipopgamento ?? "",
                                           n.id_inc
                                       };

                        //MessageBox.Show("quantidade sangria 2 " + dadosR06.Count().ToString());
                        // MessageBox.Show("data inicio " +dataInicial+ "-" + dataFinal);

                        if (dadosR06.Count() > 0)
                        {

                            foreach (var item in dadosR06)
                            {
                                #region
                                string sql = "SELECT ecffabricacao FROM `movdespesas` WHERE id_inc = '" + item.id_inc.ToString() + "' and codigoFilial = '" + GlbVariaveis.glb_filial + "'";
                                string ECFfabricacao = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                                if (ECFfabricacao == null)
                                {
                                    ECFfabricacao = ConfiguracoesECF.nrFabricacaoECF;
                                }

                                //MessageBox.Show(item.data.ToString()+" - coo"+item.ncupomfiscalCOO+" - inc"+item.id_inc.ToString());

                                sql = "SELECT modelo FROM `movdespesas` WHERE id_inc = '" + item.id_inc.ToString() + "'";
                                modeloECF = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                                if (modeloECF == null)
                                {
                                    modeloECF = "ECF FISCAL";
                                }
                                //MessageBox.Show("1");

                                sql = "SELECT mfAdicionalECF FROM `movdespesas` WHERE id_inc = '" + item.id_inc.ToString() + "'";
                                string MfAdicional = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

                                if (MfAdicional == null)
                                {
                                    MfAdicional = " ";
                                }

                                //MessageBox.Show("2");

                                var ECFModelo = modeloECF;// ConfiguracoesECF.modeloECF; // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                                var cripto = Funcoes.CriptografarMD5(
                                    item.contadornaofiscalGNF +
                                    item.ncupomfiscalCOO +
                                    item.ecfcontadorcupomfiscal +
                                    item.tipopgamento +
                                    ECFfabricacao +
                                    modeloECF +
                                    MfAdicional +
                                    string.Format("{0:yyyy-MM-dd}", item.data) +
                                    item.hora);

                                // //MessageBox.Show("3");
                                //MessageBox.Show(item.EADDados.ToString());

                                if (cripto != item.EADDados)
                                {
                                    // MessageBox.Show("301");
                                    //MessageBox.Show(ECFModelo);
                                    ECFModelo = ECFModelo.Trim().PadRight(20, '?');
                                    // MessageBox.Show("31");
                                }
                                else
                                {
                                    //MessageBox.Show("302");
                                    // MessageBox.Show(ECFModelo);
                                    ECFModelo = ECFModelo.PadRight(20, ' ').Substring(0, 20);
                                    //MessageBox.Show("32");
                                }

                                //MessageBox.Show("4");
                                conteudo.AppendLine("R06" + //01
                                    // (from n in decf where n.ecfNumero == item.ecfnumero select n.serieFabricacao).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                                    //  (from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                                    ECFfabricacao.Trim().PadRight(20, ' ').Substring(0, 20) +
                                    MfAdicional.PadRight(1, ' ').Substring(0, 1) +
                                     ECFModelo +
                                    //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                                    "01" +

                                    item.ncupomfiscalCOO.PadLeft(9, '0').Substring(0, 9) + //06
                                    item.contadornaofiscalGNF.PadLeft(6, ' ').Substring(0, 6) + //07                    
                                    "000000" +
                                    "0000" + // 09
                                    "CN" + //10
                                    string.Format("{0:yyyyMMdd}", item.data) +
                                    String.Format("{0:hhmmss}", item.hora).Replace(":", ""));

                                #endregion
                            }

                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("R06 - Sangria - " + erro.Message);
                    }



                    try
                    {
                        // Recebimentos            
                        var dadosR06RC = from n in entidade.contdocs
                                         where n.data >= dataInicial && n.data <= dataFinal
                                         && n.total > 0
                                         && n.dpfinanceiro == "Recebimento"
                                         && n.contadordebitocreditoCDC == "" && n.contadordebitocreditoCDC != null
                                         && n.ecfnumero != "" && n.ecfnumero != null
                                             //&& n.ecfnumero == numerodoECF
                                         && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                         && n.CodigoFilial == GlbVariaveis.glb_filial
                                         select new
                                         {
                                             ecffabricacao = n.ecffabricacao ?? "",
                                             ncupomfiscal = n.ncupomfiscal ?? "",
                                             COOGNF = n.COOGNF ?? "",
                                             n.data,
                                             n.hora,
                                             ecfmodelo = n.ecfmodelo ?? ""
                                         };
                        foreach (var item in dadosR06RC)
                        {
                            #region
                            var ECFModelo = item.ecfmodelo; // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                            //var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.ncupomfiscal + item.contadornaofiscalGNF + item.contadordebitocreditoCDC + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.COOGNF+ item.tipopagamento);
                            //if (cripto != item.EADr06)
                            //{
                            //    ECFModelo = "????????????????????";                    
                            //}

                            conteudo.AppendLine("R06" + //01
                             item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                             ConfiguracoesECF.mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                             ECFModelo.PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                             "01" +
                               item.ncupomfiscal.PadLeft(9, '0').Substring(0, 9) + //06
                               item.COOGNF.PadLeft(6, ' ').Substring(0, 6) + //07                    
                               "000000" +
                               "0000" + // 09
                               "CN" + //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                               string.Format("{0:yyyyMMdd}", item.data) +
                               String.Format("{0:hhmmss}", item.hora).Replace(":", ""));
                            #endregion
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("R06 - Contdocs - " + erro.Message);
                    }

                    // Comprovante de Débito e Crédito RV - Registro de Venda

                    //var dadosR06CC = from n in entidade.contdocs
                    //                 where n.data>=dataInicial && n.data<=dataFinal 
                    //                 && n.total>0
                    //                 && n.contadordebitocreditoCDC=="" && n.contadordebitocreditoCDC!=null                             
                    //                 && n.ecfnumero!="" && n.ecfnumero!=null
                    //                 && n.ecfnumero == numerodoECF
                    //                 select n;
                    //foreach (var item in dadosR06CC)
                    //{

                    //    var ECFModelo = item.ecfmodelo; // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                    //    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.ncupomfiscal + item.contadornaofiscalGNF + item.contadordebitocreditoCDC + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.COOGNF+ item.tipopagamento);
                    //    if (cripto != item.EADr06)
                    //    {
                    //        ECFModelo = "????????????????????";                    
                    //    }

                    //    conteudo.AppendLine("R06" + //01
                    //        item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                    //        //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                    //        mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                    //         ECFModelo +
                    //        //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                    //        "01"+
                    //          item.ncupomfiscal.PadRight(6, '0').Substring(0, 6) + //06
                    //          item.ecfcontadorcupomfiscal.PadRight(6, ' ').Substring(0, 6) + //07                    
                    //          "000000" +
                    //          item.contadordebitocreditoCDC.PadRight(4, '0').Substring(0, 4) + // 09
                    //          "RV"+ //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                    //          string.Format("{0:yyyyMMdd}", item.data) +
                    //          String.Format("{0:hhmmss}", item.hora).Replace(":", ""));

                    //}
                    // Relatórios Gerenciais


                    //var dadosR06CCDocs = from n in entidade.contdocs
                    //                 where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                    //                 && n.contadordebitocreditoCDC.Trim() != "" && n.contadordebitocreditoCDC != null
                    //                 && n.ecfnumero == numerodoECF && n.ecfnumero != null
                    //                 && n.total > 0
                    //                 select n;


                    //foreach (var item in dadosR06CCDocs)
                    //{
                    //    estornado = item.total <= 0 ? "S" : "N";
                    //    decimal valorEstornado = 0;
                    //    if (estornado == "S")
                    //        valorEstornado = Math.Abs(Convert.ToDecimal(item.total));

                    //    var ECFModelo = item.ecfmodelo.Trim().PadRight(20, ' ').Substring(0, 20);

                    //    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.ncupomfiscal + item.contadornaofiscalGNF + item.contadordebitocreditoCDC + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.COOGNF + item.tipopagamento + item.ecfcontadorcupomfiscal + item.ecftotalliquido.ToString().Replace(",", ".") + item.estornado);
                    //    if (cripto != item.EADr06)
                    //    {
                    //        ECFModelo = item.ecfmodelo.Trim().PadRight(20, '?');// "????????????????????";
                    //    }

                    //    conteudo.AppendLine("R06" + //01
                    //        item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                    //        //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                    //        mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                    //         ECFModelo +
                    //        //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                    //        "01"+
                    //          item.ncupomfiscal.PadRight(6, '0').Substring(0, 6) + //06
                    //          item.ecfcontadorcupomfiscal.PadRight(6, ' ').Substring(0, 6) + //07                    
                    //          "000000" +
                    //          item.contadordebitocreditoCDC.PadRight(4, '0').Substring(0, 4) + // 09
                    //          "CC"+ //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                    //          string.Format("{0:yyyyMMdd}", item.data) +
                    //          String.Format("{0:hhmmss}", item.hora).Replace(":", ""));
                    //};           

                    try
                    {
                        var dadosR06RG = (from n in entidade.contrelatoriogerencial
                                          where n.data >= dataInicial && n.data <= dataFinal
                                              /*&& n.ecfnumero == numerodoECF &&*/
                                          && n.ecfnumero != null && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                          && n.tipopagamentoECF == "RG"
                                          && n.codigofilial == GlbVariaveis.glb_filial
                                          select new
                                          {
                                              ecffabricacao = n.ecffabricacao ?? "",
                                              EADDados = n.EADDados ?? "",
                                              coo = n.coo ?? "",
                                              gnf = n.gnf ?? "",
                                              grg = n.grg ?? "",
                                              denominacao = n.denominacao ?? "",
                                              n.data,
                                              n.horaemissao,
                                              tipopagamentoECF = n.tipopagamentoECF ?? "DH",
                                              n.cdc
                                          }).Distinct();
                        foreach (var item in dadosR06RG)
                        {
                            #region
                            //var ECFModelo = (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                            var ECFModelo = modeloECF; // ConfiguracoesECF.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                            var cripto = Funcoes.CriptografarMD5(
                                item.ecffabricacao +
                                item.coo +
                                item.gnf +
                                item.tipopagamentoECF +
                                string.Format("{0:yyyy-MM-dd}", item.data.Value) +
                                item.cdc +
                                item.denominacao);

                            if (cripto != item.EADDados)
                            {
                                ECFModelo = ECFModelo.Trim().PadRight(20, '?');// "????????????????????";                   
                            }
                            else
                            {
                                ECFModelo = ECFModelo.PadRight(20, ' ').Substring(0, 20);
                            }

                            conteudo.AppendLine("R06" + //01
                                  item.ecffabricacao.Trim().PadRight(20, ' ').Substring(0, 20) +
                                // (from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                                ConfiguracoesECF.mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                                  ECFModelo +
                                //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                                  "01" +
                                  item.coo.Trim().PadLeft(9, '0').Substring(0, 9) + //06
                                  item.gnf.Trim().PadLeft(6, '0').Substring(0, 6) + //07                    
                                  item.grg.Trim().PadLeft(6, '0').Substring(0, 6) + //08 Contador Relatorio Gerencial
                                  item.cdc.Trim().PadLeft(4, '0').Substring(0, 4) +
                                  item.denominacao + //item.tipopagamentoECF + //10
                                  string.Format("{0:yyyyMMdd}", item.data) +
                                  String.Format("{0:hhmmss}", item.horaemissao).Replace(":", ""));
                            #endregion
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("R06 - Controle Gerecial " + erro.Message);
                    }


                    //var dadosR06CC = from n in entidade.contrelatoriogerencial
                    //                 where n.data >= dataInicial && n.data <= dataFinal
                    //                 && n.ecfnumero == numerodoECF && n.ecfnumero != null
                    //                 && n.tipopagamentoECF=="CC"
                    //                 select n;
                    //foreach (var item in dadosR06CC)
                    //{
                    //    //var ECFModelo = (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                    //    var ECFModelo = modeloECF; // ConfiguracoesECF.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                    //    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.coo + item.gnf + item.tipopagamentoECF + string.Format("{0:yyyy-MM-dd}", item.data) + item.cdc + item.denominacao);
                    //    if (cripto != item.EADDados)
                    //    {
                    //        ECFModelo = ECFModelo.Trim().PadRight(20, '?');// "????????????????????";                   
                    //    }

                    //    conteudo.AppendLine("R06" + //01
                    //          item.ecffabricacao.Trim().PadRight(20, ' ').Substring(0, 20) +
                    //        // (from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                    //        mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                    //          ECFModelo +
                    //        //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                    //          "01" +
                    //          item.coo.Trim().PadRight(6, '0').Substring(0, 6) + //06
                    //          item.gnf.Trim().PadRight(6, '0').Substring(0, 6) + //07                    
                    //          item.grg.Trim().PadRight(6, '0').Substring(0, 6) + //08 Contador Relatorio Gerencial
                    //          item.cdc.Trim().PadRight(4, '0').Substring(0, 4) +
                    //          item.denominacao + //item.tipopagamentoECF + //10
                    //          string.Format("{0:yyyyMMdd}", item.data) +
                    //          String.Format("{0:hhmmss}", item.horaemissao).Replace(":", ""));
                    //}

                    try
                    {

                        // Comprovante Débito Crédito
                        var dadosR06CC = from n in entidade.contdocs
                                         where n.data >= dataInicial && n.data <= dataFinal
                                         && n.total > 0
                                         && n.contadordebitocreditoCDC != "" && n.contadordebitocreditoCDC != null
                                         && n.ecfnumero != "" && n.ecfnumero != null
                                         && n.ecfnumero == numerodoECF
                                         && n.CodigoFilial == GlbVariaveis.glb_filial
                                         select new
                                         {
                                             ecffabricacao = n.ecffabricacao ?? "",
                                             COOGNF = n.COOGNF ?? "",
                                             contadornaofiscalGNF = n.contadornaofiscalGNF ?? "",
                                             n.data,
                                             n.hora,
                                             ecfmodelo = n.ecfmodelo ?? "",
                                             contadordebitocreditoCDC = n.contadordebitocreditoCDC ?? ""
                                         };

                        foreach (var item in dadosR06CC)
                        {
                            #region

                            var ECFModelo = item.ecfmodelo; // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                            //var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.ncupomfiscal + item.contadornaofiscalGNF + item.contadordebitocreditoCDC + string.Format("{0:yyyy-MM-dd}", item.data.Value) + item.COOGNF+ item.tipopagamento);
                            //if (cripto != item.EADr06)
                            //{
                            //    ECFModelo = "????????????????????";                    
                            //}

                            conteudo.AppendLine("R06" + //01
                             item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                             ConfiguracoesECF.mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                              ECFModelo.PadRight(20, ' ').Substring(0, 20) +
                                //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                             "01" +
                               item.COOGNF.PadLeft(9, '0').Substring(0, 9) + //06
                               item.contadornaofiscalGNF.PadRight(6, ' ').Substring(0, 6) + //07                    
                               "000000" +
                               item.contadordebitocreditoCDC.PadRight(4, '0').Substring(0, 4) +
                               "CC" + //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                               string.Format("{0:yyyyMMdd}", item.data) +
                               String.Format("{0:hhmmss}", item.hora).Replace(":", ""));
                            #endregion
                        }
                    }
                    catch (Exception erro)
                    {
                        throw new Exception("R06 contdoc" + erro.Message);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("R06 - " + ex.Message);
                }

                #endregion
            }

            #region R07
            //Suprimento e Saldo Inicial
            //var dadosR07Saldo = from n in entidade.caixa
            //                    where (n.tipopagamento == "SI" || n.tipopagamento == "SU")
            //                    && n.historico.Substring(2, 3) != ""
            //                    && n.historico.Substring(2, 3) == numerodoECF
            //                    && n.data>=dataInicial && n.data<=dataFinal
            //                    select n;
            //foreach (var item in dadosR07Saldo)
            //{
            //    var ECFModelo = item.ecfmodelo.Trim().PadRight(20, ' ');
            //    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.coo + item.ccf + item.gnf + item.ecfmodelo + item.valor.ToString().Replace(",",".") + item.tipopagamento);
            //    if (cripto != item.eaddados)
            //    {
            //        ECFModelo = ECFModelo.Trim().PadRight(20, '?');// "????????????????????";                   
            //    }


            //    conteudo.AppendLine("R07" + //01
            //        item.ecffabricacao.Trim().Trim().PadRight(20, ' ').Substring(0, 20) +
            //        mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
            //        ECFModelo.Trim().PadRight(20, ' ').Substring(0, 20) +
            //        "01".PadRight(2, ' ').Substring(0, 2) +                        
            //        item.historico.PadRight(6, '0').Substring(15, 6) + // COO 06
            //        "000000" + // CCF Contador Cupom Fiscal
            //        item.historico.PadRight(6, '0').Substring(7, 6) + //GNF 07                    
            //        "DH".PadRight(15, ' ') +
            //        Funcoes.FormatarZerosEsquerda(item.valor, 13,true) +
            //        "N" +
            //        Funcoes.FormatarZerosEsquerda(item.valor, 13,true));
            //}


            // Formas de pagamento
            try
            {
                var dadosR07SaldoPgt = (from n in entidade.caixatmp
                                        where (n.tipopagamento != "SI" && n.tipopagamento != "SU")
                                        && n.ecfnumero == numerodoECF
                                        && n.data >= dataInicial && n.data <= dataFinal
                                        && n.valor > 0
                                        && n.CodigoFilial == GlbVariaveis.glb_filial
                                        select new { n.eaddados, n.ecffabricacao, n.ecfmodelo, coo = n.coo ?? "", ccf = n.ccf ?? "", gnf = n.gnf ?? "", tipopagamento = n.tipopagamento ?? "", n.valor, n.data, n.tipodoc }).Concat(
                                        from n in entidade.caixa
                                        where (n.tipopagamento != "SI" && n.tipopagamento != "SU")
                                         && n.ecfnumero == numerodoECF
                                         && n.data >= dataInicial && n.data <= dataFinal
                                         && n.valor > 0
                                         && n.CodigoFilial == GlbVariaveis.glb_filial
                                        select new { n.eaddados, n.ecffabricacao, n.ecfmodelo, coo = n.coo ?? "", ccf = n.ccf ?? "", gnf = n.gnf ?? "", tipopagamento = n.tipopagamento ?? "", n.valor, n.data, n.tipodoc });
                

                foreach (var item in dadosR07SaldoPgt)
                {
                    #region

                    string formaPagamento = "Dinheiro";
                    if (item.tipopagamento == "DH")
                        formaPagamento = ConfiguracoesECF.DH;
                    if (item.tipopagamento == "CH")
                        formaPagamento = ConfiguracoesECF.CH;
                    if (item.tipopagamento == "CR")
                        formaPagamento = ConfiguracoesECF.CR;
                    if (item.tipopagamento == "CA")
                        formaPagamento = ConfiguracoesECF.CA;
                    if (item.tipopagamento == "DV")
                        formaPagamento = ConfiguracoesECF.DV;
                    if (item.tipopagamento == "TI")
                        formaPagamento = ConfiguracoesECF.TI;


                    var ECFModelo = item.ecfmodelo.Trim().PadRight(20, ' ');
                    var cripto = Funcoes.CriptografarMD5(item.ecffabricacao + item.coo + item.ccf + item.gnf + item.ecfmodelo + item.valor.ToString().Replace(",", ".") + item.tipopagamento + string.Format("{0:yyyy-MM-dd}", item.data) + item.tipodoc);
                    if (cripto != item.eaddados)
                    {
                        ECFModelo = ECFModelo.Trim().PadRight(20, '?');// "????????????????????";                   
                    }


                    conteudo.AppendLine("R07" + //01
                        item.ecffabricacao.Trim().PadRight(20, ' ').Substring(0, 20) +
                        mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                        ECFModelo.Trim().PadRight(20, ' ').Substring(0, 20) +
                        "01".PadRight(2, ' ').Substring(0, 2) +
                        item.coo.Trim().PadRight(9, '0').Substring(0, 9) + // COO 06
                        item.ccf.Trim().PadRight(9, '0').Substring(0, 9) + // CCF Contador Cupom Fiscal
                        "000000" + //GNF 07                    
                        formaPagamento.Trim().PadRight(15, ' ') +
                        Funcoes.FormatarZerosEsquerda(item.valor, 13, true) +
                        "N" +
                        Funcoes.FormatarZerosEsquerda(0, 13, true));

                    #endregion
                }



                // CDC Meio de pagamento 


                // Sangria
                //var dadosR07 = from n in entidade.movdespesas
                //               where n.data>=dataInicial && n.data<=dataFinal
                //               select n;
                //foreach (var item in dadosR07)
                //{
                //    estornado = item.valor <= 0 ? "S" : "N";
                //    decimal valorEstornado = 0;
                //    if (estornado == "S")
                //        valorEstornado = Math.Abs(item.valor);
                //    conteudo.AppendLine("R07" + //01
                //        (from n in decf where n.ecfNumero == item.ecfnumero select n.serieFabricacao).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                //        (from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +
                //        (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20) +
                //        (from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                //        item.ncupomfiscalCOO.Trim().PadRight(6, '0').Substring(0, 6) + //06
                //        item.ecfcontadorcupomfiscal.Trim().PadRight(6, '0').Substring(0, 6) + //06
                //        item.contadornaofiscalGNF.Trim().PadRight(6, '0').Substring(0, 6) + //07                    
                //        "DH".PadRight(15, ' ') +
                //        Funcoes.FormatarZerosEsquerda(item.valor, 13,true) +
                //        estornado +
                //        Funcoes.FormatarZerosEsquerda(valorEstornado, 13,true));
                //};            



                // Cancelamento de Cupom

                var dadosR07NC = from n in entidade.contdocs
                                 where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                 && n.total > 0
                                 && n.estornado == "S"
                                 && (n.ecfcontadorcupomfiscal.Trim() != "" && n.ecfcontadorcupomfiscal.Trim() != "000000")
                                 && n.dpfinanceiro == "Venda"
                                 //&& n.ecfnumero != "" && n.ecfnumero != null
                                 //&& n.ecfnumero == numerodoECF
                                 && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 select new
                                 {
                                     n.documento,
                                     n.data,
                                     n.ecffabricacao,
                                     n.ecfnumero,
                                     n.ncupomfiscal,
                                     n.ecfcontadorcupomfiscal,
                                     n.contadornaofiscalGNF,
                                     n.estornado,
                                     n.ecfmodelo,
                                     n.EADr06,
                                     n.tipopagamento,
                                     n.COOGNF,
                                     n.contadordebitocreditoCDC,
                                     n.ecftotalliquido,
                                     n.ecfMFadicional,
                                     n.Totalbruto,
                                     n.desconto,
                                     n.encargos,
                                     n.ecfConsumidor,
                                     n.ecfCPFCNPJconsumidor
                                    
                                 };

                foreach (var item in dadosR07NC)
                {
                    #region
                    var ECFModelo = item.ecfmodelo; // (from n in decf where n.ecfNumero == item.ecfnumero select n.modeloECF).First().Trim().PadRight(20, ' ').Substring(0, 20);
                    
                    /*var cripto = Funcoes.CriptografarMD5(
                        item.ecffabricacao + 
                        item.ncupomfiscal + 
                        item.contadornaofiscalGNF + 
                        item.contadordebitocreditoCDC + 
                        string.Format("{0:yyyy-MM-dd}", 
                        item.data.Value) + item.COOGNF + 
                        item.tipopagamento + 
                        item.ecfcontadorcupomfiscal + 
                        item.ecftotalliquido.ToString().Replace(",", ".") + 
                        item.estornado);*/

                    var cripto = Funcoes.CriptografarMD5(
                            item.ecffabricacao +
                            item.ncupomfiscal +
                            item.contadornaofiscalGNF +
                            item.contadordebitocreditoCDC +
                            string.Format("{0:yyyy-MM-dd}",
                            item.data.Value) +
                            item.COOGNF +
                            item.tipopagamento +
                            item.ecfcontadorcupomfiscal +
                            item.ecftotalliquido.ToString().Replace(",", ".") +
                            item.estornado +
                            item.ecfMFadicional +
                            item.ecfmodelo +
                            item.ecfcontadorcupomfiscal +
                            item.Totalbruto.ToString().Replace(",", ".") +
                            item.desconto.ToString().Replace(",", ".") +
                            item.encargos.ToString().Replace(",", ".") +
                            item.ecfConsumidor +
                            item.ecfCPFCNPJconsumidor);

                    if (cripto != item.EADr06)
                    {
                        ECFModelo = item.ecfmodelo.Trim().PadRight(20, '?');
                    }


                    siceEntities entidadeExcluir = Conexao.CriarEntidade();

                    var dadosExclusao = (from n in entidadeExcluir.caixatmp
                                         where n.documento == item.documento
                                         select new { n.tipopagamento, n.valor });


                    var dadosExclusaoDia = (from n in entidadeExcluir.caixa
                                            where n.documento == item.documento
                                            select new { n.tipopagamento, n.valor });

                    dadosExclusao.Concat(dadosExclusaoDia).ToList();

                    if (dadosExclusaoDia.Count() != 0)
                        dadosExclusao = dadosExclusaoDia;


                    #endregion

                    foreach (var itemExclusao in dadosExclusao)
                    {
                        #region

                        string formaPagamento = "Dinheiro";
                        if (itemExclusao.tipopagamento == "DH")
                            formaPagamento = ConfiguracoesECF.DH;
                        if (itemExclusao.tipopagamento == "CH")
                            formaPagamento = ConfiguracoesECF.CH;
                        if (itemExclusao.tipopagamento == "CR")
                            formaPagamento = ConfiguracoesECF.CR;
                        if (itemExclusao.tipopagamento == "CA")
                            formaPagamento = ConfiguracoesECF.CA;
                        if (itemExclusao.tipopagamento == "DV")
                            formaPagamento = ConfiguracoesECF.DV;
                        if (itemExclusao.tipopagamento == "TI")
                            formaPagamento = ConfiguracoesECF.TI;
                       

                            conteudo.AppendLine("R07" + //01
                            item.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                            //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                            item.ecfMFadicional.Trim().PadRight(1, ' ').Substring(0, 1) +
                             ECFModelo.PadRight(20, ' ').Substring(0, 20) +
                            //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                            "01" +
                              item.ncupomfiscal.PadRight(9, '0').Substring(0, 9) + //06
                              item.ecfcontadorcupomfiscal.Trim().PadRight(9, '0').Substring(0, 9) + //07                    
                              item.contadornaofiscalGNF.Trim().PadRight(6, '0').Substring(0, 6) +
                            //item.contadordebitocreditoCDC.Trim().PadRight(4, '0').Substring(0, 4) + // 09
                              formaPagamento.Trim().PadRight(15, ' ') + //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                              Funcoes.FormatarZerosEsquerda(itemExclusao.valor, 13, true) +
                              item.estornado +
                              Funcoes.FormatarZerosEsquerda(itemExclusao.valor, 13, true));

                        #endregion
                    }
                }

                // Recebimento de Clientes          
                var dadosR07RC = from n in Conexao.CriarEntidade().contdocs
                                 where n.data >= dataInicial.Date && n.data <= dataFinal.Date
                                 && n.total > 0
                                 && n.dpfinanceiro == "Recebimento"
                                 && n.contadordebitocreditoCDC == "" && n.contadordebitocreditoCDC != null
                                 && n.ecfnumero != "" && n.ecfnumero != null
                                 //&& n.ecfnumero == numerodoECF
                                 && n.ecffabricacao == ConfiguracoesECF.nrFabricacaoECF
                                 && n.CodigoFilial == GlbVariaveis.glb_filial
                                 select new
                                 {
                                     ecffabricacao = n.ecffabricacao ?? "",
                                     ncupomfiscal = n.ncupomfiscal ?? "",
                                     COOGNF = n.COOGNF ?? "",
                                     n.data,
                                     n.hora,
                                     ECFModelo = n.ecfmodelo ?? "",
                                     n.total,
                                     n.tipopagamentoECF,
                                     n.estornado,
                                     n.tipopagamento
                                 };

                var dadosR07Lista = dadosR07RC.ToList();

                foreach (var itemR07RC in dadosR07Lista)
                {
                    #region

                    string formaPagamento = "Dinheiro";
                    if (itemR07RC.tipopagamento == "DH")
                        formaPagamento = ConfiguracoesECF.DH;
                    if (itemR07RC.tipopagamento == "CH")
                        formaPagamento = ConfiguracoesECF.CH;
                    if (itemR07RC.tipopagamento == "CR")
                        formaPagamento = ConfiguracoesECF.CR;
                    if (itemR07RC.tipopagamento == "CA")
                        formaPagamento = ConfiguracoesECF.CA;
                    if (itemR07RC.tipopagamento == "DV")
                        formaPagamento = ConfiguracoesECF.DV;
                    if (itemR07RC.tipopagamento == "TI")
                        formaPagamento = ConfiguracoesECF.TI;

                    conteudo.AppendLine("R07" + //01
                      itemR07RC.ecffabricacao.PadRight(20, ' ').Substring(0, 20) +
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.MFAdicional).First().Trim().PadRight(1, ' ').Substring(0, 1) +                    
                      mfAdicionalECF.Trim().PadRight(1, ' ').Substring(0, 1) +
                       itemR07RC.ECFModelo.PadRight(20, ' ').Substring(0, 20) +
                        //(from n in decf where n.ecfNumero == item.ecfnumero select n.usuarioSubECF).First().PadRight(2, ' ').Substring(0, 2) +
                      "01" +
                        itemR07RC.ncupomfiscal.PadRight(9, '0').Substring(0, 9) + //06
                        "0".PadRight(9, '0').Substring(0, 9) + //07                    
                        itemR07RC.COOGNF.Trim().PadRight(6, '0').Substring(0, 6) +
                        //item.contadordebitocreditoCDC.Trim().PadRight(4, '0').Substring(0, 4) + // 09
                        formaPagamento.Trim().PadRight(15, ' ') + //item.tipopagamentoECF.PadRight(2,' ').Substring(0,2) + //10
                        Funcoes.FormatarZerosEsquerda(itemR07RC.total.Value, 13, true) +
                        itemR07RC.estornado +
                        Funcoes.FormatarZerosEsquerda(0, 13, true));
                    #endregion
                }

            }
            catch (Exception ex)
            {
                throw new Exception("R07 - " + ex.Message);
            }

            #endregion


            return conteudo;
        }
        #endregion
        
        #region V1 -Identificação do Usuário do PAF-ECF

        public StringBuilder V1() // Identificação do Usuário do PAF-ECF
        {

            StringBuilder conteudo = new StringBuilder();

            var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);
            conteudo.AppendLine("V1" +
                Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                razao);

            conteudo.AppendLine("V2" +
               GlbVariaveis.cnpjSH.Trim().PadRight(14, '0').Substring(0, 14) +
               GlbVariaveis.IESH.Trim().PadRight(14, ' ').Substring(0, 14) +
               GlbVariaveis.IEMunicipalSH.Trim().PadRight(14, ' ').Substring(0, 14) +
               GlbVariaveis.razaoSH);

            conteudo.AppendLine("V3" +
              GlbVariaveis.laudoPAF.Trim().PadRight(10, ' ').Substring(0, 10) +
              GlbVariaveis.nomeAplicativo.Trim().PadRight(50, ' ').Substring(0, 50) +
              GlbVariaveis.versaoPAF.Trim().PadRight(10, ' ').Substring(0, 10) +
              GlbVariaveis.razaoSH);

            var ecfs = from n in Conexao.CriarEntidade().r01
                       select n;

            var dadosEcfs = ecfs.ToList();
            int nRegistro = 0;
            foreach (var item in dadosEcfs)
            {
             conteudo.AppendLine("V4" +
             item.fabricacaoECF.Trim().PadRight(20, ' ').Substring(0, 20) +
             item.MFAdicional.Trim().PadRight(1, ' ').Substring(0, 1) +
             item.marcaECF.Trim().PadRight(20, ' ').Substring(0, 20) +
             item.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20));
               nRegistro++;
            }

            conteudo.AppendLine("V9" +
              GlbVariaveis.cnpjSH.Trim().PadRight(14, '0').Substring(0, 14) +
              GlbVariaveis.IESH.Trim().PadRight(14, ' ').Substring(0, 14) +
              GlbVariaveis.versaoPAF.Trim().PadRight(10, ' ').Substring(0, 10) +
              string.Format("{0:000000}", nRegistro));                     
            return conteudo;
        }
        #endregion

        #region Z1 - Vendas identificadas pelo CPF/CNPJ
        public StringBuilder Z1(int tipo,string CPFCNPJ, string ano ,string mes) //  VENDAS IDENTIFICADAS PELO CPF/CNPJ 
        {
            StringBuilder conteudo = new StringBuilder();

            try
            {
                var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);
                conteudo.AppendLine("Z1" +
                    Configuracoes.cnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                    Configuracoes.inscricao.Trim().PadRight(14, ' ').Substring(0, 14) +
                    Configuracoes.inscricaoMunicipal.Trim().PadRight(14, ' ').Substring(0, 14) +
                    razao);

                conteudo.AppendLine("Z2" +
                  GlbVariaveis.cnpjSH.Trim().PadRight(14, '0').Substring(0, 14) +
                  GlbVariaveis.IESH.Trim().PadRight(14, ' ').Substring(0, 14) +
                  GlbVariaveis.IEMunicipalSH.Trim().PadRight(14, ' ').Substring(0, 14) +
                  GlbVariaveis.razaoSH);

                conteudo.AppendLine("Z3" +
                  GlbVariaveis.laudoPAF.Trim().PadRight(10, ' ').Substring(0, 10) +
                  GlbVariaveis.nomeAplicativo.Trim().PadRight(50, ' ').Substring(0, 50) +
                  GlbVariaveis.versaoPAF.Trim().PadRight(10, ' ').Substring(0, 10));

                //Obtendo valores por CPF/CNPJ
                string sql = "";
                if (tipo == 1)
                {
                    sql = "SELECT ecfCPFCNPJconsumidor AS cpfcnpj,SUM(totalbruto) AS total ,MIN(DATA) dataInicial, MAX(DATA) dataFinal FROM contdocs WHERE ecfCPFCNPJconsumidor IS NOT NULL " +
                        "AND ecfCPFCNPJconsumidor = '" + CPFCNPJ + "'  AND codigoFilial = '" +GlbVariaveis.glb_filial+"' "+
                        "GROUP BY ecfCPFCNPJconsumidor;";
                }
                else
                {
                    sql = "SELECT ecfCPFCNPJconsumidor AS cpfcnpj,SUM(totalbruto) AS total ,MIN(DATA) dataInicial, MAX(DATA) dataFinal FROM contdocs WHERE ecfCPFCNPJconsumidor IS NOT NULL " +
                            "AND ecfCPFCNPJconsumidor<>'' " +
                            "AND codigofilial = '"+GlbVariaveis.glb_filial+"' "+
                            "AND MONTH(DATA)='" + mes + "' AND YEAR(DATA)='" + ano + "' " +
                            "GROUP BY ecfCPFCNPJconsumidor;";
                }
                var dados = Conexao.CriarEntidade().ExecuteStoreQuery<VendaPorCPFCNPJ>(sql).ToList();
                int nRegistros = 0;
                foreach (var item in dados)
                {
                   conteudo.AppendLine("Z4" +
                   item.cpfcnpj.Trim().PadRight(14, '0').Substring(0, 14) +
                   Funcoes.FormatarZerosEsquerda(item.total, 14, true) +
                   String.Format("{0:yyyyMMdd}", item.dataInicial) +
                   String.Format("{0:yyyyMMdd}", item.dataFinal) +
                   String.Format("{0:yyyyMMdd}", DateTime.Now.Date) +
                   String.Format("{0:HHMMss}", DateTime.Now));
                    nRegistros++;
                }
                conteudo.AppendLine("Z9" +
                  GlbVariaveis.cnpjSH.Trim().PadRight(14, '0').Substring(0, 14) +
                  GlbVariaveis.IESH.Trim().PadRight(14, ' ').Substring(0, 14) +
                  string.Format("{0:000000}", nRegistros));
                return conteudo;
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message);
                return conteudo;
            }
        }
        #endregion

        #region  GERAÇÃO E TRANSMISSÃO AUTOMÁTICA DE INFORMAÇÕES E ARQUIVOS REDUÇÃO Z
        public bool ReducaoZXML(bool criarTabelaVenda, DateTime dataInicial, DateTime dataFinal, string numerodoECF, string nomeArquivoDestino, bool fechamentoZ = true)
        {

            XDocument xml = new XDocument(new XDeclaration("1.0", "UTF-8", "?"));

            XElement ReducaoZ = new XElement("ReducaoZ", new XAttribute("Versao", "1.0"));

            XElement Mensagem = new XElement("Mensagem");

            //XElement reducaoZVersao = new XElement("ReducaoZ", new XAttribute("Versao", "1.0"));

            Mensagem.Add(new XElement("Estabelecimento",// new XComment(" comentario de registro")                        
                    new XElement("Ie", Configuracoes.inscricao),
                    new XElement("Cnpj", Configuracoes.cnpj),
                    new XElement("NomeEmpresarial", Configuracoes.razaoSocial)));

            XElement PafEcf = new XElement("PafEcf",
                new XElement("NumeroCredenciamento", GlbVariaveis.laudoPAF.PadLeft(15, '0')),
                new XElement("NomeComercial", GlbVariaveis.nomeAplicativo),
                new XElement("Versao", GlbVariaveis.versaoPAF),
                new XElement("CnpjDesenvolvedor", GlbVariaveis.cnpjSH),
                new XElement("NomeEmpresarialDesenvolvedor", GlbVariaveis.nomeSH));

            XElement Ecf = new XElement("Ecf");
            XElement DadosReducaoZ = new XElement("DadosReducaoZ");
            XElement TotalizadoresParciais = new XElement("TotalizadoresParciais");
            XElement ProdutosServicos = new XElement("ProdutosServicos");
            //XElement Servico = new XElement("Servico");

            #region Ecf

            // Criando o arquivo temporário das vendas
            if (criarTabelaVenda)
            {
                Funcoes.CriarTabelaTmp("venda", dataInicial.Date, dataFinal.Date, GlbVariaveis.glb_filial, false);
                Funcoes.CriarTabelaTmp("caixa", dataInicial.Date, dataFinal.Date, GlbVariaveis.glb_filial, false);
            }

            Funcoes.ProcedureAjuste("AjustarCamposNulos");


            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;

            string modeloECF = " ";
            string fabricacaoECF = " ";
            string mfAdicionalECF = " ";
            try
            {
                var dadosR01 = (from n in entidade.r01
                                orderby n.data descending
                                select n).ToList().Take(100).Distinct();

                if (dadosR01.Count() == 0)
                {
                    dadosR01 = (from n in entidade.r01
                                where n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                orderby n.data descending
                                select n).Take(1);
                }

                if (fechamentoZ)
                {
                    dadosR01 = (from n in dadosR01
                                where n.numeroECF == numerodoECF
                                && n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                select n).Take(1);

                    if (dadosR01.Count() == 0)
                    {
                        dadosR01 = (from n in entidade.r01
                                    where n.numeroECF == numerodoECF
                                    && n.fabricacaoECF == ConfiguracoesECF.nrFabricacaoECF
                                    select n).Take(1);
                    }
                }

                modeloECF = dadosR01.First().modeloECF;
                fabricacaoECF = dadosR01.First().fabricacaoECF ?? " ";
                mfAdicionalECF = ConfiguracoesECF.mfAdicionalECF; // dadosR01.First().MFAdicional ?? " ";


                foreach (var r1 in dadosR01)
                {
                    /// Criar uma List com os dados dos ECF's para serem usados a seguir
                    /// 

                    var ECFModelo = r1.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                    var ECFModeloR1 = r1.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);

                    if (r1.EADdados != Funcoes.CriptografarMD5(r1.fabricacaoECF + r1.cnpj + r1.cnpjdesenvolvedora + r1.aplicativo + r1.md5))
                    {
                        ECFModelo = r1.modeloECF.Trim().PadRight(20, '?'); //"????????????????????";
                        ECFModeloR1 = r1.modeloECF.Trim().PadRight(20, '?');
                    }

                    var razao = Configuracoes.razaoSocial.Trim().PadRight(50, ' ').Substring(0, 50);

                    string razaosocialdesenvolvedora = r1.razaosocialdesenvolvedora.Trim().PadRight(40, ' ').Substring(0, 40);
                    if (!FuncoesPAFECF.VerificarQtdRegistro("vendaarquivo"))
                    {
                        ECFModelo = r1.modeloECF.Trim().PadRight(20, '?');
                    }

                    //aqui Implementar r01

                    Ecf.Add(new XElement("NumeroFabricacao", r1.fabricacaoECF),
                        new XElement("Tipo", r1.tipoECF),
                        new XElement("Marca", r1.marcaECF),
                        new XElement("Modelo", r1.modeloECF),
                        new XElement("Versao", r1.versaoSB),
                        new XElement("Caixa", r1.numeroECF));
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar elemento ecf do XML - " + ex.Message);
            }
            #endregion

            #region Dados ReduçãoZ
            try
            {
                var dadosR02 = from n in entidade.r02
                               where n.data >= dataInicial && n.data <= dataFinal
                               && n.numeroECF == numerodoECF
                               select n;
                foreach (var r2 in dadosR02)
                {
                    var ECFModeloR2 = r2.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                    if (r2.EADdados != Funcoes.CriptografarMD5(r2.fabricacaoECF + r2.crz + r2.coo + r2.cro + string.Format("{0:yyyy-MM-dd}", r2.data) + string.Format("{0:yyyy-MM-dd}", r2.dataemissaoreducaoz.Value) + r2.horaemissaoreducaoz + r2.vendabrutadiaria.ToString().Replace(",", ".")))
                        ECFModeloR2 = r2.modeloECF.Trim().PadRight(20, '?'); //ECFModeloR2 =  "????????????????????";

                    DadosReducaoZ.Add(new XElement("DataReferencia", string.Format("{0:yyyy-MM-dd}", r2.datamovimento)),
                        new XElement("CRZ", r2.crz),
                        new XElement("COO", r2.coo),
                        new XElement("CRO", r2.cro),
                        new XElement("VendaBrutaDiaria", r2.vendabrutadiaria.ToString().Replace(".", ",")),
                        new XElement("GT", r2.gtfinal.ToString().Replace(".", ",")));
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Dados reducao XML - " + ex.Message);
            }
            #endregion

            #region Totalizador Parcial
            try
            {
                var dadosR03 = from n in entidade.r03
                               where n.data >= dataInicial && n.data <= dataFinal
                               && n.numeroECF == numerodoECF
                               && n.valoracumulado > 0
                               && n.totalizadorParcial != "OPNF"
                               && n.totalizadorParcial != "AT"
                               && !n.totalizadorParcial.StartsWith("Can")
                               && !n.totalizadorParcial.StartsWith("D")
                               && n.codigofilial == GlbVariaveis.glb_filial
                               select n;

                foreach (var r3 in dadosR03)
                {
                    var ECFmodelo = r3.modeloECF.Trim().PadRight(20, ' ').Substring(0, 20);
                    var cripto = Funcoes.CriptografarMD5(r3.fabricacaoECF + r3.CRZ + r3.totalizadorParcial.ToString().Replace(",", "."));
                    if (cripto != r3.EADdados)
                        ECFmodelo = r3.modeloECF.Trim().PadRight(20, '?');

                    string tributacao = verificaTributacao(r3.totalizadorParcial);
                    int icms = verificaAliquota(r3.totalizadorParcial);

                    var objR05 = from n in Conexao.CriarEntidade().r05
                                 where n.data == r3.data.Value
                                 && n.ecfnumero == numerodoECF
                                 && n.cancelado == "N"
                                 && n.ecffabricacao == r3.fabricacaoECF
                                 && n.tributacao == tributacao
                                 && n.icms == icms
                                 orderby n.documento
                                 select new
                                 {
                                     n.icms,
                                     n.tributacao,
                                     n.cancelado,
                                     n.ecfmodelo,
                                     n.documento,
                                     n.eaddados,
                                     ecffabricacao = n.ecffabricacao ?? " ",
                                     n.ecfcontadorcupomfiscal,
                                     n.ecfMFadicional,
                                     n.codigo,
                                     n.produto,
                                     n.ncupomfiscal,
                                     n.unidade,
                                     n.descontovalor,
                                     n.quantidade,
                                     n.nrcontrole,
                                     n.indicadorarredondamentotruncamento,
                                     n.indicadorproducao,
                                     n.preco,
                                     n.total,
                                     n.Descontoperc,
                                     n.data,
                                     n.acrescimototalitem,
                                     n.coo,
                                     n.ccf,
                                     n.estornado,
                                     n.canceladoECF
                                 };

                    var dadosR05 = objR05.ToList();

                    if (dadosR05.Count() > 0)
                    {
                        foreach (var item in dadosR05)
                        {
                            ProdutosServicos.Add(new XElement("Produto",
                            new XElement("Descricao", item.produto),
                            new XElement("Codigo", item.codigo),
                            new XElement("CodigoTipo", "Proprio"),
                            new XElement("Quantidade", item.quantidade.ToString("N2").Replace(".", ",")),
                            new XElement("Unidade", item.unidade.Substring(0, 2)),
                            new XElement("ValorUnitario", item.preco.Value.ToString("N2").Replace(".", ","))));
                        }
                    }

                    TotalizadoresParciais.Add(new XElement("TotalizadorParcial",
                        new XElement("Nome", r3.totalizadorParcial),
                        new XElement("Valor", r3.valoracumulado.ToString().Replace(".", ",")), ProdutosServicos));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Totalizador Parcial XML - " + ex.Message);
            }
            #endregion

            Mensagem.Add(PafEcf);
            //ProdutosServicos.Add(Servico);                
            //TotalizadoresParciais.Add(ProdutosServicos);                    
            DadosReducaoZ.Add(TotalizadoresParciais);
            Ecf.Add(DadosReducaoZ);
            Mensagem.Add(Ecf);
            ReducaoZ.Add(Mensagem);
            xml.Add(ReducaoZ);
            xml.Save(@nomeArquivoDestino);

            try
            {
                var certificado = CertificadoDigital.SelecionarCertificado();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@nomeArquivoDestino);
                XmlDocument xmlAssinado = CertificadoDigital.Assinar(xmlDoc, "ReducaoZ", certificado);
                System.IO.File.Delete(@nomeArquivoDestino);
                xmlAssinado.Save(@nomeArquivoDestino);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            return true;
        }

        private string verificaTributacao(string totalizadorR03)
        {
            if (totalizadorR03.Contains("T18") == true || totalizadorR03.Contains("T17") || totalizadorR03.Contains("T12") || totalizadorR03.Contains("T27") || totalizadorR03.Contains("T07") || totalizadorR03.Contains("T25"))
                return "00";
            else if (totalizadorR03.Contains("N1") == true)
                return "41";
            else if (totalizadorR03.Contains("I1") == true)
                return "40";
            else if (totalizadorR03.Contains("F1") == true)
                return "60";
            else
                return "90";

        }

        private int verificaAliquota(string totalizadorR03)
        {
            string aliquota = totalizadorR03.Replace("01T", "").Replace("02T", "").Replace("03T", "").Replace("04T", "").Replace("05T", "").Replace("06T", "").Replace("07T", "").Replace("08T", "");
            aliquota = aliquota.Replace("1T", "").Replace("02T", "").Replace("3T", "").Replace("4T", "").Replace("5T", "").Replace("6T", "").Replace("7T", "").Replace("8T", "");
            aliquota = aliquota.Replace("00", "");
            try
            {
                return int.Parse(aliquota);
            }
            catch (Exception erro)
            {
                return 0;
            }

        }


        #endregion

        #region Geração e transmissão autmática envio de estoque

        public bool EnvioEstoqueXML(DateTime dataInicial,DateTime dataFinal,string nomeArquivoDestino, string tabelaPreco="varejo",string tipo="T")
        {

            siceEntities entidade = Conexao.CriarEntidade();
            entidade.CommandTimeout = 3600;

            string marcado = " ";
            if (tipo.ToLower() == "T" || tipo.ToLower()=="total")
            {
                string sql = "UPDATE produtos set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);
                sql = "UPDATE produtosfilial set marcado=' '";
                Conexao.CriarEntidade().ExecuteStoreCommand(sql);

            }
            if (tipo == "P" || tipo=="parcial" )
                marcado = "P";


            
                        XDocument xml = new XDocument(new XDeclaration("1.0", "UTF-8", "?"));      
            
                        XElement Estoque = new XElement("Estoque", new XAttribute("Versao","1.0"));
            
                        XElement Mensagem = new XElement("Mensagem");
            
                        //XElement reducaoZVersao = new XElement("ReducaoZ", new XAttribute("Versao
            
                        Mensagem.Add(new XElement("Estabelecimento",// new XComment(" comentario de
                                new XElement("Ie", Configuracoes.cnpj),
                                new XElement("Cnpj", Configuracoes.inscricao ),
                                new XElement("NomeEmpresarial",Configuracoes.razaoSocial)));
            
                        XElement PafEcf = new XElement("PafEcf",
                            new XElement("NumeroCredenciamento", GlbVariaveis.laudoPAF),
                            new XElement("NomeComercial", GlbVariaveis.nomeAplicativo),
                            new XElement("Versao", GlbVariaveis.versaoPAF),
                            new XElement("CnpjDesenvolvedor", GlbVariaveis.cnpjSH),
                            new XElement("NomeEmpresarialDesenvolvedor", GlbVariaveis.nomeSH));


                        XElement DadosEstoque = new XElement("DadosEstoque");
                        XElement produtos = new XElement("Produtos");


                         DadosEstoque.Add(new XElement("DataReferenciaInicial", DateTime.Now.Date),
                              new XElement("DataReferenciaFinal",DateTime.Now.Date));   
   

                        var itens = (from n in entidade.produtos
                            where n.CodigoFilial == GlbVariaveis.glb_filial
                            && n.marcado==marcado
                            select new
                            {
                                n.codigo,
                                n.descricao,
                                n.unidade,
                                n.saldofinalestoque,
                                n.unidembalagem,
                                n.indicadorarredondamentotruncamento,
                                n.indicadorproducao,
                                n.tributacao,
                                n.tipo,
                                n.icms,
                                n.precovenda,
                                n.precoatacado,
                                n.EADP2relacaomercadoria,
                                n.STecf
                            })
                           .Concat(
                           from n in entidade.produtosfilial
                           where n.CodigoFilial == GlbVariaveis.glb_filial
                           && n.marcado==marcado
                           select new
                           {
                               n.codigo,
                               n.descricao,
                               n.unidade,
                               n.saldofinalestoque,
                               n.unidembalagem,
                               n.indicadorarredondamentotruncamento,
                               n.indicadorproducao,
                               n.tributacao,
                               n.tipo,
                               n.icms,
                               n.precovenda,
                               n.precoatacado,
                               n.EADP2relacaomercadoria,
                               n.STecf
                           });
            


            if (tabelaPreco == "varejo")
            {
                itens = from n in itens
                           where n.precovenda > 0
                           select n;
            }

            if (tabelaPreco == "atacado")
            {
                itens = from n in itens
                           where n.precoatacado > 0
                           select n;
            }

            foreach (var item in itens)
            {
                /*
                 * I = Isenção
                 * N = Não incidência
                 * F = Substituição Tributária
                 */

                try
                {
                    decimal preco = item.precovenda;
                    string unidade = item.unidade;

                    var tributacao = SituacaoTributacao(Convert.ToInt16(item.icms), item.tributacao, item.tipo);

                    if (tabelaPreco == "atacado")
                    {
                        preco = item.precoatacado;
                        unidade = item.unidembalagem;
                    }

                    produtos.Add(new XElement("Produto",
                             new XElement("Descricao", item.descricao),
                             new XElement("CodigoInterno", item.codigo, new XAttribute("tipo", "GTIN")),
                             new XElement("Quantidade", item.saldofinalestoque),
                             new XElement("Unidade", unidade ),
                             new XElement("ValorUnitario",preco),
                             new XElement("SituacaoTributaria","T"),
                             new XElement("Aliquota",tributacao),
                             new XElement("IndicadorArredondamento",item.indicadorarredondamentotruncamento),
                             new XElement("Ippt",item.indicadorproducao),
                             new XElement("SituacaoEstoque",tipo)));                     
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            };

            Mensagem.Add(PafEcf);
            DadosEstoque.Add(produtos);
            Mensagem.Add(DadosEstoque);
            Estoque.Add(Mensagem);
            xml.Add(Estoque);     
            xml.Save(@nomeArquivoDestino);

            try
            {
                var certificado = CertificadoDigital.SelecionarCertificado();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@nomeArquivoDestino);
                XmlDocument xmlAssinado = CertificadoDigital.Assinar(xmlDoc, "Estoque", certificado);
                System.IO.File.Delete(@nomeArquivoDestino);
                xmlAssinado.Save(@nomeArquivoDestino);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }                        
            return true;
        }

        #endregion

        private string SituacaoTributacao(int icms, string tributacao, string tipo)
        {
            //            7.2.1.5 - Campo 08: Tabela de Situações Tributárias: http://www.fazenda.gov.br/confaz/confaz/atos/atos_cotepe/2008/ac006_08.htm

            //CódigoSituação Tributária
            //I
            //Isento
            //N
            //Não Tributado
            //F
            //Substituição Tributária
            //T
            //Tributado pelo ICMS
            //S
            //Tributado pelo ISSQN

            if (tributacao == "41")
                return "I"; // Isenção

            if (tributacao == "40")
                return "I"; // Isenção
            //icms == 0 || 
            //    icms == 0 || 


            if (tributacao == "10" || tributacao == "30" || tributacao == "60" || tributacao == "70")            
                return "F"; // Substituição Tributária
            if (tributacao == "80")
                return "N"; // Não Incidência
            if (tipo == "1")
                return "S";

            return "T";

        }
    }

    public class VendaPorCPFCNPJ
    {
        public string cpfcnpj { get; set; }
        public decimal total { get; set; }
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
    }

    public class N1
    {
        public string cnpj { get; set; }
        public string inscricao { get; set; }
        public string inscricaoMunicipal { get; set; }
        public string razao { get; set; }
        public string chave { get; set; }
    }

}