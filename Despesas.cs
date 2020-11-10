using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.EntityClient;
using System.Data;
using System.Globalization;

namespace SICEpdv
{
    class Despesas
    {

        public List<String> Contas(string filial)
        {                        
           List<String> listaContas = new List<string>();

           siceEntities entidade;
           if (Conexao.tipoConexao == 2)
               entidade = Conexao.CriarEntidade(false);
           else
               entidade = Conexao.CriarEntidade();

            //siceEntities entidade = Conexao.CriarEntidade();

            var contas = from c in entidade.despesas   
                         where c.codigofilial == GlbVariaveis.glb_filial
                         select new
                         {
                             conta = c.conta,
                             descricao = c.descricao                             
                         };
            if (contas.Count() == 0)
                throw new Exception("Não existem contas cadastradas para esta filial !");


            foreach (var item in contas)
            {
              listaContas.Add(item.conta+" - "+item.descricao);
                
            };
            return listaContas;
        }

        public List<string> SubContas(string filial, string conta,bool mostrarRestritas)
        {                   
            conta=conta.PadLeft(5,'0');            
            List<String> listaSubContas = new List<string>();


            siceEntities entidade;
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();


            //siceEntities entidade = Conexao.CriarEntidade() ;

            var subcontas = from c in entidade.despesasub
                            where c.idconta == conta && c.liberada == "S"
                            && c.codigofilial == filial
                            select new
                            {
                                subConta=c.idsubconta,
                                descricao=c.descricao
                            };

            if (subcontas.Count() == 0)
                throw new Exception("Não existem sub-contas cadastradas para esta filial !");

            foreach (var item in subcontas)
            {
                listaSubContas.Add(item.subConta + " - " + item.descricao);
            }            
            return listaSubContas;
        }

        public static bool ContaCredito(string filial,string tipo, string conta, string subConta)
        {                        
            //siceEntities entidade = Conexao.CriarEntidade();

             siceEntities entidade;
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();


            switch (tipo)
            {
                case "CR":
                    var cr = (from c in entidade.despesasub
                              where c.idconta== conta && c.idsubconta == subConta
                              && c.codigofilial == filial
                              select c.creditobancario).FirstOrDefault();
                    return cr == "S" ? true : false;
                    
                case "DB":
                    var db = (from c in entidade.despesasub
                              where c.idconta == conta && c.idsubconta == subConta
                              && c.codigofilial == filial
                              select c.debitobancario).FirstOrDefault();
                    return db == "S" ? true : false;
            }
            return false;
        }
       
        public void Lancar(decimal valor,string conta,string subConta,string contaBancaria,
                         long cheque,bool credito,bool debito, string historico,string tipoPagamento,
            int devolucaoNumero,string operadorDestino)
        {
            FuncoesECF fecf = new FuncoesECF();
            string interporlador = string.Format("{0:hh:mm:ss}", DateTime.Now);
                
            if (valor <= 0)
                    throw new Exception("Valor incorreto !");
                
            if (valor>25000)
                 throw new Exception("Sangria limitada a 25.000,00 por vez !");

                        
            siceEntities entidade = Conexao.CriarEntidade();

            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);


            var q = from n in entidade.despesasub
                    where n.idconta == conta && n.idsubconta == subConta
                    && n.codigofilial == GlbVariaveis.glb_filial
                    select n;

            if (q.Count() == 0)
                throw new Exception("Conta e Sub-Conta não encontrada");

            foreach (var condicao in q)
            {
                if ( (condicao.creditobancario == "S" || condicao.debitobancario == "S") && (contaBancaria == "" || contaBancaria == null) )
                    throw new Exception("Sub-Conta exige escolha de uma conta bancária");

                if ((credito==true || debito==true) && (contaBancaria == "" || contaBancaria == null))
                    throw new Exception("Foi escolhido a movimentação bancária porém nenhuma conta foi escolhida");                
            }
            try
            {                
                fecf.SangriaECF(valor);                              
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
           
            var conteudo = "";
            var dataHoraCupomECF = FuncoesECF.DataHoraUltDocumentoECF();

            string vendedorDevolucao = "000";
            if (devolucaoNumero > 0)
            {
                vendedorDevolucao = (from v in Conexao.CriarEntidade().devolucao
                                    where v.numero == devolucaoNumero
                                    select v.vendedor).FirstOrDefault();
            }

            movdespesas item = new movdespesas();
            item.id = GlbVariaveis.glb_IP;
            item.grupo = "";
            
            item.codigofilial = GlbVariaveis.glb_filial;
            item.data = dataHoraCupomECF == null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
            item.hora = dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
            item.valor = valor;
            item.conta = conta;
            item.subconta = subConta;            
            item.historico = historico;
           
            item.ncupomfiscalCOO = FuncoesECF.COONumeroCupomFiscal(ConfiguracoesECF.idECF).ToString();
            item.ecfcontadorcupomfiscal = FuncoesECF.CCFContadorCupomECF();
            item.contadornaofiscalGNF = FuncoesECF.ContadorNaoFiscalGNF();
            item.ecfnumero = FuncoesECF.NumeroECF(ConfiguracoesECF.idECF);
            item.tipopgamento = tipoPagamento;             
            item.operador = GlbVariaveis.glb_Usuario;
            item.devolucaonumero = devolucaoNumero;
            
            item.descricaoconta = (from n in entidade.despesas
                                   where n.conta == conta
                                   && n.codigofilial == GlbVariaveis.glb_filial
                                   select n.descricao).First();
            item.descricaosubconta = (from n in entidade.despesasub
                                      where n.idconta == conta && n.idsubconta == subConta
                                      && n.codigofilial == GlbVariaveis.glb_filial
                                      select n.descricao).First();

            item.despesa = (from n in entidade.despesasub
                            where n.idconta == conta && n.idsubconta == subConta
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select n.despesa).First();

            item.sangria = "S";
            item.contabancaria = contaBancaria;
            item.interpolador = interporlador;
            item.tipodespesa = (from n in entidade.despesasub
                                where n.idconta == conta && n.idsubconta == subConta
                                && n.codigofilial == GlbVariaveis.glb_filial
                                select n.tipodespesa).First();
            item.vendedorcomissao = vendedorDevolucao;
            item.cobradorcomissao = "000";
            item.encerrado = "N";
            item.OpCxAdm = "";
            item.EADDados = Funcoes.CriptografarMD5(item.contadornaofiscalGNF + item.ncupomfiscalCOO + item.ecfcontadorcupomfiscal + item.tipopgamento);
            entidade.AddTomovdespesas(item);

            conteudo = item.conta + " " + item.descricaoconta + Environment.NewLine;
            conteudo += item.subconta + " " + item.descricaosubconta + Environment.NewLine;
            conteudo += item.historico+Environment.NewLine;
            conteudo += "Valor R$ : "+ item.valor + Environment.NewLine;


            entidade.SaveChanges();

            if (Conexao.tipoConexao == 2 || Conexao.tipoConexao == 3)
                StandAlone.salvarHistorico("sangria");

            // Lançamento Movimentação Bancária

            try
            {
                int id = (from d in entidade.movdespesas
                                  where d.codigofilial == GlbVariaveis.glb_filial && d.id == GlbVariaveis.glb_IP
                                  select d.id_inc).Max();

                var movDespesas = (from d in entidade.movdespesas
                                   where d.id_inc == id
                                   select d).FirstOrDefault();

                movDespesas.ecffabricacao = ConfiguracoesECF.nrFabricacaoECF;
                movDespesas.modelo = ConfiguracoesECF.modeloECF;
                movDespesas.mfAdicionalECF = ConfiguracoesECF.mfAdicionalECF;
                entidade.SaveChanges();

                //string sql = "SELECT MAX(v.id_inc) as codigo FROM movdespesas AS v WHERE v.codigofilial='" + GlbVariaveis.glb_filial + "' AND v.id = '" + GlbVariaveis.glb_IP + "' LIMIT 1";
                //int id = Conexao.CriarEntidade().ExecuteStoreQuery<int>(sql).FirstOrDefault();
                //sql = "UPDATE movdespesas AS m SET m.`ecffabricacao` =  '" + ConfiguracoesECF.nrFabricacaoECF + "', modelo = '" + ConfiguracoesECF.modeloECF.Trim() + "', mfAdicionalECF = '"+ConfiguracoesECF.mfAdicionalECF.Trim()+"' WHERE m.id_inc = '" + id + "'";
                //Conexao.CriarEntidade().ExecuteStoreCommand(sql);

                string sql = "UPDATE movdespesas SET EADDados = MD5(CONCAT(contadornaofiscalGNF, ncupomfiscalCOO, ecfcontadorcupomfiscal, tipopgamento, ECFfabricacao,modelo ,MfAdicionalECF,DATA,hora)) WHERE `id_inc` =  '" + id + "'";
                entidade.ExecuteStoreCommand(sql);

            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.ToString());
            }

           
            if (credito)
            {
                try
                {
                    movcontasbanco movBanco = new movcontasbanco();
                    movBanco.conta = contaBancaria;
                    movBanco.movimento = "credito";
                    movBanco.valorcredito = valor;
                    movBanco.data = GlbVariaveis.Sys_Data.Date;
                    movBanco.historico = "Caixa: Depósito";
                    movBanco.codigofilial = GlbVariaveis.glb_filial;
                    movBanco.codigofilial = GlbVariaveis.glb_Usuario;
                    movBanco.interpolador = interporlador;
                    entidade.AddTomovcontasbanco(movBanco);
                    movBanco.cpmfcalculado = "N";
                    entidade.SaveChanges();
                }
                catch (Exception erro)
                {
                    throw new Exception("Erro ao creditar a Conta Bancária. " + erro.Message);
                }
            }
            if (debito)
            {
                try
                {
                    movcontasbanco movBanco = new movcontasbanco();
                    movBanco.conta = contaBancaria;
                    movBanco.movimento = "debito";
                    movBanco.valordebito = valor;
                    movBanco.data = GlbVariaveis.Sys_Data.Date;
                    movBanco.historico = "Caixa: Pagamento de Despesa com cheque";
                    movBanco.codigofilial = GlbVariaveis.glb_filial;
                    movBanco.codigofilial = GlbVariaveis.glb_Usuario;
                    movBanco.interpolador = interporlador;
                    movBanco.cheque = cheque ;
                    movBanco.cpmfcalculado = "N";
                    entidade.AddTomovcontasbanco(movBanco);
                    entidade.SaveChanges();                    
                }
                catch
                {
                    throw new Exception ("Erro ao debitar conta bancária ");
                }
            }

            // Processando Devolucao
            if (devolucaoNumero > 0)
            {
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.ProcessarDevolucao";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter doc = cmd.Parameters.Add("doc", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = 0;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter nrDevolucao = cmd.Parameters.Add("devolucaoNR", DbType.Int32);
                    nrDevolucao.Direction = ParameterDirection.Input;
                    nrDevolucao.Value = devolucaoNumero;

                    EntityParameter operador = cmd.Parameters.Add("operadorAcao", DbType.String);
                    operador.Direction = ParameterDirection.Input;
                    operador.Value = GlbVariaveis.glb_Usuario;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            

            if (!string.IsNullOrEmpty(operadorDestino))
            {
                /*entidade = Conexao.CriarEntidade();
                caixa lancar = new caixa();
                lancar.tipopagamento = "SU";
                lancar.valor = valor;
                lancar.CodigoFilial = GlbVariaveis.glb_filial;
                lancar.filialorigem = GlbVariaveis.glb_filial;
                lancar.operador = operadorDestino;
                lancar.EnderecoIP = GlbVariaveis.glb_IP;
                lancar.dpfinanceiro = "Saldo inicial";
                lancar.data = DateTime.Now.Date;  //== null ? DateTime.Now.Date : Convert.ToDateTime(dataHoraCupomECF).Date;
                lancar.horaabertura = DateTime.Now.TimeOfDay; // dataHoraCupomECF == null ? DateTime.Now.TimeOfDay : Convert.ToDateTime(dataHoraCupomECF).TimeOfDay;
                lancar.versao = GlbVariaveis.glb_Versao;
                lancar.historico = "*";
                lancar.vendedor = "000";
                // Aqui é usado o histórico para gravar o número do ECF para tirar o 
                // relatório R07 com os documentos não fiscais.
                // Onde: E=NumeroEcF g=Contador Nao Fiscal GNF e C=COO numero do cupom emitido
                lancar.historico = "TR Transferência "+interporlador;
                entidade.AddTocaixa(lancar);
                entidade.SaveChanges();   */
                //valor = Convert.ToDecimal(valor, new CultureInfo("en-US"));
                string valorSangria = Convert.ToDecimal(valor, new CultureInfo("en-US")).ToString();

                string sql = "call finalizarTransfCaixa('"+GlbVariaveis.glb_filial+"','"+GlbVariaveis.glb_IP+"','"+GlbVariaveis.glb_Usuario+"','"+ operadorDestino +"','"+ valorSangria.Replace(",",".") + "', 0);";
                entidade.ExecuteStoreCommand(sql);

            }

            // Imprimindo segunda via no ECF
            FuncoesECF.RelatorioGerencial("abrir", "");
            FuncoesECF.RelatorioGerencial("imprimir", conteudo);
            FuncoesECF.RelatorioGerencial("fechar", "");

            if(ConfiguracoesECF.NFC == true)
            {
                conteudo += "->>>> 2º Via da Sangria! <<<<-" + Environment.NewLine;
                FuncoesECF.RelatorioGerencial("abrir", "");
                FuncoesECF.RelatorioGerencial("imprimir", conteudo);
                FuncoesECF.RelatorioGerencial("fechar", "");
            }

        }

        public bool Estornar(int id,decimal valor)
        {
            FuncoesECF fecf = new FuncoesECF();
            if (!fecf.SuprimentoECF(valor))
                throw new Exception("Não foi possível imprimir suprimento no ECF. Tente novamente !!");

            try
            {
            siceEntities entidade = Conexao.CriarEntidade();
            var excluir = (from n in entidade.movdespesas
                           where n.id_inc == id
                           select n).FirstOrDefault();
                // Estorna a devolução caso a sangria tenha sido devolução
            if (excluir.devolucaonumero > 0)
            {
                using (EntityConnection conn = new EntityConnection(Conexao.stringConexao))
                {
                    conn.Open();
                    EntityCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "siceEntities.EstornarDevolucao";
                    cmd.CommandType = CommandType.StoredProcedure;

                    EntityParameter doc = cmd.Parameters.Add("doc", DbType.Int32);
                    doc.Direction = ParameterDirection.Input;
                    doc.Value = 0;

                    EntityParameter filial = cmd.Parameters.Add("filial", DbType.String);
                    filial.Direction = ParameterDirection.Input;
                    filial.Value = GlbVariaveis.glb_filial;

                    EntityParameter ip = cmd.Parameters.Add("ipTerminal", DbType.String);
                    ip.Direction = ParameterDirection.Input;
                    ip.Value = GlbVariaveis.glb_IP;

                    EntityParameter nrDevolucao = cmd.Parameters.Add("devolucaoNR", DbType.Int32);
                    nrDevolucao.Direction = ParameterDirection.Input;
                    nrDevolucao.Value = excluir.devolucaonumero;

                    EntityParameter operador = cmd.Parameters.Add("operadorAcao", DbType.String);
                    operador.Direction = ParameterDirection.Input;
                    operador.Value = GlbVariaveis.glb_Usuario;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

            var interpolador = excluir.interpolador;
            entidade.DeleteObject(excluir);
            var excluirMovBanco = (from n in entidade.movcontasbanco
                                   where n.interpolador == interpolador
                                   select n);                
            if (excluirMovBanco.Count()>0)
            entidade.DeleteObject(excluirMovBanco.First());

            var excluirCaixa = (from n in entidade.caixa
                                where n.historico.Contains(interpolador)
                                select n);
            if (excluirCaixa.Count()>0)
                entidade.DeleteObject(excluirCaixa.First());

            entidade.SaveChanges();

            }                
            catch (Exception erro)
            {

                throw new Exception(erro.Message);
            }
            
            return true;
        }
    }
}
