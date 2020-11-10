using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Entity;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace SICEpdv
{
    public class Operador
    {
        public static bool autorizado { get; set; }
        public static string ultimoOperadorAutorizado { get; set; }
        
        /// <summary>
        /// se a variável campo for = a operador entao só verificar se a senha
        /// pertence ao operador. Não verifique a autorização do campo
        /// </summary>
        /// <param name="operador"></param>
        /// <param name="senha"></param>
        /// <param name="campo"></param>
        /// <returns></returns>
        public static bool Autorizacao(string operador, string senha,string campo)
        {
            
            // SE estiver OFF line sempre retorna true pois só é possível alterar 
            // dados local e vender somente venda a vista
            if (!Conexao.onLine)
                return true;

            autorizado = false;        
            senha = Funcoes.CriptografarMD5(senha);

            

            if (operador.Contains("0359"))
                {                                
                    try
                    {
                     string sql = "SELECT operador FROM senhas WHERE credenciaispermissao='" + Funcoes.CriptografarMD5(operador) + "' AND senhas." + campo + "='S'";                    
                    operador = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();


                    if (!string.IsNullOrEmpty(operador))
                    {
                        autorizado = true;
                        return autorizado;
                    }
                    else
                    {
                        autorizado = false;
                        return autorizado;
                    }

                    }
                    catch (Exception)
                    {
                    return false;
                    }

                }
            

           EntityConnection conn = new EntityConnection(Conexao.stringConexao);

           if (Conexao.tipoConexao == 2)
               conn = new EntityConnection(Conexao.stringConexao);

            using (conn)
            {
                Int16 codigo = 0;
                try
                {
                    codigo = Convert.ToInt16(operador);
                }
                catch
                {
                    codigo = 0;
                };



                conn.Open();
                EntityCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select senhas." + campo + ",senhas.operador from " +
                                   "siceEntities.senhas " +
                                   "where (senhas.operador=@operador or senhas.codigo=@codigo) and senhas.senha=@senha";                                   
                EntityParameter param = new EntityParameter();
                param.ParameterName = "operador";
                param.Value = operador;
               
                EntityParameter param1 = new EntityParameter();
                param1.ParameterName = "senha";
                param1.Value = senha;

                EntityParameter param2 = new EntityParameter();
                param2.ParameterName = "codigo";
                param2.Value = codigo;

                cmd.Parameters.Add(param);
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);

                EntityDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                while (reader.Read())
                {
                    if (campo != "operador")
                        autorizado = reader[0].ToString() == "S" ? true : false;
                    else
                        autorizado = true;

                    if (autorizado)
                    ultimoOperadorAutorizado = reader[1].ToString();
                }
                conn.Close();
            }
            
            return autorizado;
        }

        private void Aguarde()
        {
            FrmMsgOperador frmmsg = new FrmMsgOperador("", "Preparando ambiente, aguarde...");
            frmmsg.ShowDialog();
            frmmsg.Dispose();
        }


        public bool Login(string operador, string senha)
        {            
            Int16 codigo = 0;
            try
            {
                codigo = Convert.ToInt16(operador);
            }
            catch
            {
                codigo = 0;
            };

            GlbVariaveis.glb_senhaUsuario = senha;
            senha = Funcoes.CriptografarMD5(senha);


            /// If operandus modus = StandAlone
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                using (IObjectContainer tabela = Db4oFactory.OpenFile(@"senhas.yap"))
                {
                    var userOff = from StandAloneUsuario n in tabela
                                  where (n.operador == operador.ToUpper() || n.codigo == codigo) && n.senha == senha
                                  select new
                                  {
                                      operador = n.operador,
                                  };

                    if (userOff.Count() == 0)
                    {
                        throw new Exception("Operador não foi encontrado.");
                    };

                    autorizado = true;
                    GlbVariaveis.glb_Usuario = userOff.First().operador;
                    tabela.Close();
                    return true;
                };
            };
            
            try
            {
                siceEntities entidade = Conexao.CriarEntidade();
                if (Conexao.tipoConexao == 2)
                    entidade = Conexao.CriarEntidade(false);

                if (operador.Contains("0359"))
                {
                    try
                    {                       
                        string sql = "SELECT operador FROM senhas WHERE credenciaispermissao='" + Funcoes.CriptografarMD5(operador) + "'";
                        string sql2 = "SELECT senha FROM senhas WHERE credenciaispermissao='" + Funcoes.CriptografarMD5(operador) + "'";
                        operador = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();
                        senha = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql2).FirstOrDefault();                       
                    }
                    catch (Exception)
                    {
                        throw new Exception("Credencias de acesso negada");
                    }
                 
                }

               
                var usuario = from n in entidade.senhas
                              where (n.operador == operador.ToUpper() || n.codigo == codigo) && n.senha == senha
                              select new
                              {
                                  operador = n.operador,
                                  codFilial = n.CodigoFilial,
                                  horaInicial = n.horainiciartrabalho,
                                  horaFinal = n.horafinalizartrabalho
                              };


                if (usuario.Count() == 0)
                {
                    throw new Exception("Operador não foi encontrado");
                }

                foreach (var item in usuario)
                {
                    if (DateTime.Now.TimeOfDay < item.horaInicial || DateTime.Now.TimeOfDay > item.horaFinal)
                        throw new Exception("Atenção você não pode acessar o sistema neste horário ");

                    autorizado = true;
                    ultimoOperadorAutorizado = operador;
                    //if (string.IsNullOrEmpty(GlbVariaveis.glb_Usuario))
                        GlbVariaveis.glb_Usuario = item.operador;
                }
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
           
            return autorizado;
        }

    }
}


