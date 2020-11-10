using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    class Vendedor
    {
        public static string codigoVendedor { get; set; }
        public static string nomeVendedor { get; set; }

        public static void VendaVendedor(string codigo)
        {
            codigoVendedor = codigo;
            nomeVendedor = (from n in Conexao.CriarEntidade().vendedores
                            where n.codigo == codigo
                            && n.CodigoFilial == GlbVariaveis.glb_filial
                            select n.nome).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(nomeVendedor))
            {
            Venda.vendedor="000";
            nomeVendedor="Geral";
            return;
            }

            Venda.vendedor = codigo;                        
            siceEntities entidade = Conexao.CriarEntidade();
            List<int> id = new List<int>();

            var lst = (from n in entidade.vendas
                       where n.id == GlbVariaveis.glb_IP
                       && n.codigofilial == GlbVariaveis.glb_filial
                       select n.inc).ToList();

            id.AddRange(lst);

            foreach (var item in id)
            {
                var dados = (from n in entidade.vendas
                             where n.inc == item
                             select n).First();
                dados.vendedor = Venda.vendedor;
                entidade.SaveChanges();
            }

        }

        public static void VendedorAssociado(string operador)
        {
            if (!Conexao.onLine && Conexao.tipoConexao == 1)
            {
                codigoVendedor = "000";
                nomeVendedor = "Geral";
                return;
            }

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var vendedor = (from n in entidade.senhas
                           where n.operador == operador && n.codigovendedor!="000"
                           select n.codigovendedor).FirstOrDefault();            

            if (!string.IsNullOrEmpty(vendedor))
            {
                try
                {
                    codigoVendedor = (from n in entidade.vendedores
                                      where n.codigo == vendedor
                                      select n.codigo).FirstOrDefault();

                    nomeVendedor = (from n in entidade.vendedores
                                    where n.codigo == vendedor
                                    select n.nome).FirstOrDefault();
                    Venda.vendedor = codigoVendedor;
                }
                catch
                {
                codigoVendedor = "000";
                nomeVendedor = "Geral";
                Venda.vendedor = "000";
                throw new Exception("Verifique se o vendedor associado ao operador existe no cadastro de vendedores. ");
                }
            }
            else
            {
                codigoVendedor = "000";
                nomeVendedor = "Geral";
                Venda.vendedor = "000";
            }

        }
    }


}
