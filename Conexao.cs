using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Db4objects.Db4o;
using System.IO;
using System.Data.Common;
using Db4objects.Db4o.Linq;
using System.Data;
using System.Data.Objects;

namespace SICEpdv
{
    public class Conexao
    {            
        public static string stringConexao { get; set; }
        public static string stringConexaoRemoto { get; set; }
        public static string ipServidor { get; private set; }
        public static bool onLine { get; set; } // Usado no modo Standalone feito por Ivan para a homologação
        public static int tipoConexao { get; set; } // Usado um banco de dados local feito por Marckvaldo para ser usado principalmente em mercados. Flag no ConfigPaf
        

        public static string ObterStringConexao(bool onLine= true)
        {
            if (onLine == true)
            {

               if (stringConexao != null)
                    return stringConexao;

                if(Configuracoes.configuracaoSetada == false)
                    stringConexao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(ConfigurationManager.ConnectionStrings["siceEntities"].ConnectionString), GlbVariaveis.glbSenhaIQ);
                else
                {
                    System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(@"SICEpdv.exe");
                    stringConexao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(config.ConnectionStrings.ConnectionStrings["siceEntities"].ConnectionString), GlbVariaveis.glbSenhaIQ);
                }
                // substitui o localhost pelo numero paa que a regular expression 
                // consiga entender o ip do servidor  

                stringConexao = stringConexao.Replace("localhost", "127.0.0.1");
                // Expressao regular de IP válido em seguida extrai o ip da string de conexão
                // com isso é possível descobrir o ip do servidor
                Regex reg = new Regex(@"\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?");
                Match a = reg.Match(stringConexao);
                while (a.Success)
                {
                    ipServidor = a.Value;
                    a = a.NextMatch();
                }
                //if (!VerificaConexaoDB())
                //    stringConexao = ConfigurationManager.ConnectionStrings["PAF"].ConnectionString;

               // LogSICEpdv.Registrarlog(stringConexao,"","Conexao");
                return stringConexao;

            }
            else
            {
                    if (stringConexaoRemoto != null)
                        return stringConexaoRemoto;

                    if (Configuracoes.configuracaoSetada == false)
                        stringConexaoRemoto = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(ConfigurationManager.ConnectionStrings["siceEntitiesLocal"].ConnectionString), GlbVariaveis.glbSenhaIQ);
                    else
                    {
                        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(@"SICEpdv.exe");
                        stringConexao = Funcoes.DesCriptografarComSenha(Convert.FromBase64String(config.ConnectionStrings.ConnectionStrings["siceEntitiesLocal"].ConnectionString), GlbVariaveis.glbSenhaIQ);
                    }

                    // substitui o localhost pelo numero paa que a regular expression 
                    // consiga entender o ip do servidor  

                    stringConexaoRemoto = stringConexaoRemoto.Replace("localhost", "127.0.0.1");
                    // Expressao regular de IP válido em seguida extrai o ip da string de conexão
                    // com isso é possível descobrir o ip do servidor
                    Regex reg = new Regex(@"\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?");
                    Match a = reg.Match(stringConexaoRemoto);
                    while (a.Success)
                    {
                        ipServidor = a.Value;
                        a = a.NextMatch();
                    }
                    //if (!VerificaConexaoDB())
                    //    stringConexao = ConfigurationManager.ConnectionStrings["PAF"].ConnectionString;

                    return stringConexaoRemoto;
            }

         }        

        public static siceEntities CriarEntidade(bool onLine = true)
        {            
            siceEntities entidade = new siceEntities(ObterStringConexao(onLine));
            entidade.CommandTimeout = 3600;
            return entidade;
        }

        public static bool VerificaConexaoDB()
        {
          
            if (Conexao.tipoConexao == 1)//normal on e stadalone com as tabelas de yap que ivan criou
            {
                try
                {

                    if (stringConexao == null)
                        ObterStringConexao();
                   
                    var testeConexao = (from n in CriarEntidade().filiais
                                        select n.CodigoFilial).First();

                    onLine = true;
                    return true;
                }
                catch(Exception erro)
                {

                    if (File.Exists("produtos.yap") && Produtos.ProdutosOFF == null)
                    {
                        IObjectContainer tabela = Db4oFactory.OpenFile("produtos.yap");
                        Produtos.ProdutosOFF = (from StandAloneProdutos n in tabela
                                                orderby n.codigo
                                                select n).ToList();
                        tabela.Close();
                    }
                    onLine = false;
                    return false;
                }
            }
            else // nessa opção todas as vendas são feitas localmente na base de dados mysql depois elas são sincronizadas
            {
                // string teste = ConfigurationManager.ConnectionStrings["siceEntities"].ToString();

                if (stringConexao == null)
                    ObterStringConexao();

                if (stringConexaoRemoto == null)
                    ObterStringConexao(false);


                try
                {
                    var testeConexao = (from n in CriarEntidade().filiais
                                        select n.CodigoFilial).First();
                }
                catch (Exception erro)
                {
                    MessageBox.Show("SICEpdv em modo Off-line");
                    //MessageBox.Show(erro.ToString());
                }

                try
                {
                    var testeConexaoRemotamente = (from n in CriarEntidade(false).filiais
                                                    select n.CodigoFilial).First();
                }
                catch(Exception)
                {
                    //stringConexaoRemoto = ObterStringConexao();
                    MessageBox.Show("Não foi possivel conectar com a matriz!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                onLine = true;
                return true;
            }
                    

            // Aqui o PDV só fica no modo StandAlone se for um PDV
            // Para qualquer outra função sempre será on line. Com isso 
            // se houver queda da rede apresentará uma mensagem de erro.
            //if (ConfiguracoesECF.pdv)
            //onLine = false;

            ////stringConexao = null;
            ////ObterStringConexao();
            //return false;
        }

        public static bool ConexaoOnline()
        {
            try
            {
                if (Conexao.tipoConexao == 2)
                {

                    siceEntities entidade = Conexao.CriarEntidade();
                    var codigoFilial = (from f in entidade.filiais where f.CodigoFilial == "00001" select f.CodigoFilial).FirstOrDefault();
                    return true;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }


        public static void ExecuteSql(ObjectContext c, string sql)
        {
            var entityConnection = (System.Data.EntityClient.EntityConnection)c.Connection;
            DbConnection conn = entityConnection.StoreConnection;
            ConnectionState initialState = conn.State;
            try
            {
                if (initialState != ConnectionState.Open)
                    conn.Open();  // open connection if not already open
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (initialState != ConnectionState.Open)
                    conn.Close(); // only close connection if not initially open
            }
        }
    }
}
