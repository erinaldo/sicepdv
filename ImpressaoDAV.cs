using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using Microsoft.Reporting;

namespace SICEpdv
{
    public partial class ImpressaoDAV : Form
    {
        int numeroDAV;
        public ImpressaoDAV(int numero)
        {
            numeroDAV = numero;
            InitializeComponent();
        }

        private void ImpressaoDAV_Load(object sender, EventArgs e)
        {
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
            ImprimirDAV();

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                ImprimirDAVOS();
        }

        private void ImprimirDAV()
        {
            siceEntities entidade = Conexao.CriarEntidade();

            var dadosDAV = (from n in entidade.contdav
                            where n.numeroDAVFilial == numeroDAV
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select n).First();


            var dados = from n in entidade.vendadav
                        where n.documento == numeroDAV
                        && n.codigofilial == GlbVariaveis.glb_filial
                        && n.cancelado == "N"
                        orderby n.inc
                        //&& n.cancelado == "N"
                        select new
                        {
                            codigo = n.codigo,
                            produto = n.produto,/*+"["+(GlbVariaveis.glb_filial == "00001" ? (from p in entidade.produtos where p.codigo == n.codigo select p.localestoque).FirstOrDefault() : (from p in entidade.produtosfilial where p.codigo == n.codigo && p.CodigoFilial == GlbVariaveis.glb_filial select p.localestoque).FirstOrDefault()) +"]",*/
                            unidade = n.unidade,
                            quantidade = n.quantidade,
                            precooriginal = n.precooriginal,
                            Descontoperc = n.Descontoperc,
                            descontovalor = n.descontovalor,
                            acrescimototalitem = n.acrescimototalitem, //(n.preco - n.precooriginal) <= 0 ? 0 : n.preco - n.precooriginal,
                            total = n.total,
                            tipo = n.tipo,
                            cancelado = n.cancelado
                        };

            var pagamentos = from n in entidade.caixadav
                             where n.documento == numeroDAV
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             orderby n.tipopagamento, n.vencimento
                             select new
                             {
                                 Nrparcela = n.Nrparcela,
                                 tipopagamento = n.tipopagamento,
                                 vencimento = n.vencimento,
                                 valor = n.valor,
                             };

            decimal? totalServicos = (from n in dados
                                      where n.tipo == "1 - Servico"
                                      select (decimal?)n.total).Sum();

            decimal? descontoServicos = (from n in dados
                                         where n.tipo == "1 - Servico"
                                         select (decimal?)n.descontovalor).Sum();
            string dadosIdcliente = "";

            var sql = "select IFNULL(ecfconsumidor, '') as cliente from contdav where numerodavfilial = " + numeroDAV;
            var nomeCliente = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

            sql = "select IFNULL(`foneEntrega` , '')  as foneEntrega from contdav where numerodavfilial = " + numeroDAV;
            var fone = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

            sql = "select IFNULL(`fone1Entrega` , '') as fone1Entrega from contdav where numerodavfilial = " + numeroDAV;
            var fone1 = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

            sql = "select IFNULL(`complementoEntrega` , '') as complementoEntrega from contdav where numerodavfilial = " + numeroDAV;
            var complemento = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

            sql = "select IFNULL(`email` , '') as email from contdav where numerodavfilial = " + numeroDAV;
            var email = Conexao.CriarEntidade().ExecuteStoreQuery<String>(sql).FirstOrDefault();

            sql = "select IFNULL(`troco` , '0') as troco from contdav where numerodavfilial = " + numeroDAV;
            string troco = Conexao.CriarEntidade().ExecuteStoreQuery<string>(sql).FirstOrDefault();

            dadosIdcliente = nomeCliente + " - " + Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ");


//            dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ");


            if (dadosDAV.codigocliente > 0)
            {

                var dadosClientes = (from n in entidade.clientes
                                     where n.Codigo == dadosDAV.codigocliente
                                     select new { n.endereco, n.numero, n.cep, n.bairro, n.cidade, n.estado, n.telefone, n.celular }).FirstOrDefault();

                string end = dadosClientes.endereco ?? "";
                string num = dadosClientes.numero ?? "";
                string cep = dadosClientes.cep ?? "";
                string bai = dadosClientes.bairro ?? "";
                string cid = dadosClientes.cidade ?? "";
                string est = dadosClientes.estado ?? "";
                string tel = dadosClientes.telefone.Replace("_", "").Replace("-", "") ?? "";
                string cel = dadosClientes.celular.Replace("_","").Replace("-","") ?? "";

                dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ") +
                 end + " " + num + Environment.NewLine +
                 cep + " " + bai + Environment.NewLine +
                 cid + " " + est + Environment.NewLine +
                 "Tel: " + Funcoes.FormatarTelefone(tel)+" " + Funcoes.FormatarTelefone(cel) ;

            }

            string infoServico = "";
            string infoEntrega = "";
            if (totalServicos.HasValue)
                infoServico = "Produtos/Peças : " + string.Format("{0:N2}", dadosDAV.valor - totalServicos.GetValueOrDefault()) + "       Serviços R$: " + string.Format("{0:N2}", totalServicos.GetValueOrDefault()) + "   Desc.Serviço R$: " + string.Format("{0:N2}", descontoServicos.GetValueOrDefault());

            if (dadosDAV.enderecoentrega != "")
            {
                infoEntrega = "Dados Adicionais / End.Entrega" + Environment.NewLine +
                    "Reponsavel.: "+ dadosDAV.responsavelreceber +  Environment.NewLine +
                    "Endereço.:  "+dadosDAV.enderecoentrega + ", " + ((dadosDAV.numeroentrega == "" || dadosDAV.numeroentrega == null) ? "S/N" : dadosDAV.numeroentrega) + " Comp.: "+complemento + Environment.NewLine +
                     dadosDAV.cepentrega + ", " + dadosDAV.bairroentrega + ", " + dadosDAV.cidadeentrega + " - " + dadosDAV.estadoentrega + Environment.NewLine +
                    "Contatos.: "+Funcoes.FormatarTelefone(fone) + " - " + Funcoes.FormatarTelefone(fone1) + " - " +email + Environment.NewLine +
                    "OBS .:";
            }
            string dadosDAVOS = " ";

            if (!string.IsNullOrEmpty(dadosDAV.osnrfabricacao))
            {
                dadosDAVOS = "PRODUTO OBJETO DO CONSERTO " + Environment.NewLine + Environment.NewLine +
                             "NR. FABRICAÇÃO:" + dadosDAV.osnrfabricacao + Environment.NewLine + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(dadosDAV.placa))
            {
                dadosDAVOS += "PRODUTO OBJETO DO CONSERTO" + Environment.NewLine + Environment.NewLine +
                    "Marca  : " + dadosDAV.marca + " Modelo: " + dadosDAV.modelo + Environment.NewLine +
                    "Ano    : " + dadosDAV.osnrfabricacao + " PLACA : " + dadosDAV.placa + Environment.NewLine +
                    "RENAVAM: " + dadosDAV.renavam;
            }

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                infoEntrega += Environment.NewLine + Configuracoes.textoGarantia;

            vendadavBindingSource.DataSource = dados;
            caixadavBindingSource.DataSource = pagamentos;
            string descricaoTitulo = "DOCUMENTO AUXILIAR DE VENDA ORÇAMENTO";
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                descricaoTitulo = "ORÇAMENTO ORDEM DE SERVIÇO DAV-OS";

            ReportParameter titulo = new ReportParameter("lblTitulo", descricaoTitulo);
            ReportParameter empresa = new ReportParameter("lblEmpresa", Configuracoes.razaoSocial);
            ReportParameter dadosEmpresa = new ReportParameter("lblDadosEmpresa", Funcoes.FormatarCNPJ(Configuracoes.cnpj) +
                                            Environment.NewLine + Configuracoes.endereco +
                                            Environment.NewLine + Configuracoes.bairro + " " + Configuracoes.cidade + " " + Configuracoes.estado +
                                            Environment.NewLine + "Tel:" + Funcoes.FormatarTelefone(Configuracoes.telefone));
            ReportParameter idCliente = new ReportParameter("lblCliente", dadosDAV.codigocliente + " - " + dadosDAV.cliente);
            
            ReportParameter dadosCliente = new ReportParameter("lblDadosCliente", dadosIdcliente);

            string SQLB = "";
            string SQLL = "";
            if (GlbVariaveis.glb_filial == "00001")
            {
                SQLB = "(SELECT " +
                             " IFNULL(SUM(p.pesobruto * v.quantidade), 0) AS bruto " +
                             " FROM vendadav AS v, produtos AS p " +
                             " WHERE v.documento = '" + numeroDAV + "'  " +
                             " AND v.codigo = p.codigo " +
                             " AND v.codigofilial = '" + GlbVariaveis.glb_filial + "')";

                SQLL = "(SELECT " +
                      " IFNULL(SUM(p.pesoliquido * v.quantidade), 0) AS bruto " +
                      " FROM vendadav AS v, produtos AS p " +
                      " WHERE v.documento = '" + numeroDAV + "'  " +
                      " AND v.codigo = p.codigo " +
                      " AND v.codigofilial = '" + GlbVariaveis.glb_filial + "')";
            }
            else
            {
                SQLB = "(SELECT " +
                             " IFNULL(SUM(p.pesobruto * v.quantidade), 0) AS bruto " +
                             " FROM vendadav AS v, produtosfilial AS p " +
                             " WHERE v.documento = '" + numeroDAV + "'  " +
                             " AND v.codigo = p.codigo " +
                             " AND v.codigofilial = '" + GlbVariaveis.glb_filial + "')";

                SQLL = "(SELECT " +
                      " IFNULL(SUM(p.pesoliquido * v.quantidade), 0) AS bruto " +
                      " FROM vendadav AS v, produtosfilial AS p " +
                      " WHERE v.documento = '" + numeroDAV + "'  " +
                      " AND v.codigo = p.codigo " +
                      " AND v.codigofilial = '" + GlbVariaveis.glb_filial + "')";
            }

            var resultPesoBruto = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(SQLB).FirstOrDefault();
            var resultPesoLiquido = Conexao.CriarEntidade().ExecuteStoreQuery<decimal>(SQLL).FirstOrDefault();

            ReportParameter pesoBruto = new ReportParameter("lblPesoBruto", resultPesoBruto.ToString("N2"));
            ReportParameter pesoLiquido = new ReportParameter("lblPesoLiquido", resultPesoLiquido.ToString("N2"));

            ReportParameter davNumero = new ReportParameter("lblDAV", Funcoes.FormatarZerosEsquerda(numeroDAV, 10, false));
            ReportParameter totalBruto = new ReportParameter("lblTotalBruto", (dadosDAV.valor + dadosDAV.desconto).ToString());
            ReportParameter totalDesconto = new ReportParameter("lblTotalDesconto", (dadosDAV.desconto).ToString());
            ReportParameter encargos = new ReportParameter("lblTotalEncargos", (dadosDAV.encargos).ToString());
            ReportParameter totalLiquido = new ReportParameter("lblTotal", (dadosDAV.valor + dadosDAV.encargos).ToString());
            ReportParameter obs = new ReportParameter("lblObs", dadosDAV.observacao);
            ReportParameter servicos = new ReportParameter("lblServicos", infoServico);
            ReportParameter entrega = new ReportParameter("lblEnderecoEntrega", infoEntrega);
            ReportParameter davOS = new ReportParameter("davOS", dadosDAVOS);
            ReportParameter lblTroco = new ReportParameter("lblTroco", "R$"+ troco);

            var dadosVendedor = "";
            if (dadosDAV.vendedor != "000")
                dadosVendedor = (from n in entidade.vendedores where n.codigo == dadosDAV.vendedor select n.codigo + " " + n.nome).First().ToString();

            ReportParameter vendedor = new ReportParameter("lblVendedor", dadosVendedor ?? "");
            ReportParameter operador = new ReportParameter("lblOperador", "OPER: " + GlbVariaveis.glb_Usuario);
            rptDAV.LocalReport.SetParameters(new ReportParameter[] { titulo, empresa, dadosEmpresa, idCliente, dadosCliente, davNumero, totalBruto, totalDesconto, encargos, totalLiquido, operador, obs, servicos, entrega, vendedor, davOS, pesoBruto, pesoLiquido, lblTroco });
            this.rptDAV.RefreshReport();
            //reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);            
        }

        private void ImprimirDAVOS()
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dadosDAV = (from n in entidade.contdavos
                            where n.numeroDAVFilial == numeroDAV
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select n).First();


            var dados = from n in entidade.vendadavos
                        where n.documento == numeroDAV
                        && n.codigofilial == GlbVariaveis.glb_filial
                        && n.cancelado == "N"
                        orderby n.produto
                        select new
                        {
                            codigo = n.codigo,
                            produto = n.produto,
                            unidade = n.unidade,
                            quantidade = n.quantidade,
                            precooriginal = n.precooriginal,
                            Descontoperc = n.Descontoperc,
                            descontovalor = n.descontovalor,
                            acrescimototalitem = n.acrescimototalitem, //(n.preco - n.precooriginal) <= 0 ? 0 : n.preco - n.precooriginal,
                            total = n.total,
                            tipo = n.tipo
                        };
            var pagamentos = from n in entidade.caixadavos
                             where n.documento == numeroDAV
                             && n.CodigoFilial == GlbVariaveis.glb_filial
                             orderby n.tipopagamento, n.vencimento
                             select new
                             {
                                 Nrparcela = n.Nrparcela,
                                 tipopagamento = n.tipopagamento,
                                 vencimento = n.vencimento,
                                 valor = n.valor,
                             };
            decimal? totalServicos = (from n in dados
                                      where n.tipo == "1 - Servico"
                                      select (decimal?)n.total).Sum();
            decimal? descontoServicos = (from n in dados
                                         where n.tipo == "1 - Servico"
                                         select (decimal?)n.descontovalor).Sum();
            string dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ");


            if (dadosDAV.codigocliente > 0)
            {
                var dadosClientes = (from n in entidade.clientes
                                     where n.Codigo == dadosDAV.codigocliente
                                     select new { n.endereco, n.numero, n.cep, n.bairro, n.cidade, n.estado }).FirstOrDefault();
                dadosIdcliente = Funcoes.FormatarCPF(dadosDAV.ecfCPFCNPJconsumidor ?? " ") +
                 Environment.NewLine + dadosClientes.endereco ?? " " + " " + dadosClientes.numero ?? " " +
                 Environment.NewLine + dadosClientes.cep ?? " " + " " + dadosClientes.bairro ?? " " +
                 Environment.NewLine + dadosClientes.cidade ?? " " + " " + dadosClientes.estado ?? " ";

            }

            string infoServico = "";
            string infoEntrega = "";
            if (totalServicos.HasValue)
                infoServico = "Produtos/Peças : " + string.Format("{0:N2}", dadosDAV.valor - totalServicos.GetValueOrDefault()) + "       Serviços R$: " + string.Format("{0:N2}", totalServicos.GetValueOrDefault()) + "   Desc.Serviço R$: " + string.Format("{0:N2}", descontoServicos.GetValueOrDefault());

            if (dadosDAV.enderecoentrega != "")
            {
                infoEntrega = "Dados Adicionais / End.Entrega" + Environment.NewLine +
                    dadosDAV.responsavelreceber + Environment.NewLine +
                    dadosDAV.enderecoentrega + " " + dadosDAV.numeroentrega + Environment.NewLine +
                    dadosDAV.cepentrega + " " + dadosDAV.bairroentrega + " " + dadosDAV.cidadeentrega + dadosDAV.estadoentrega + Environment.NewLine +
                    dadosDAV.horaentrega.ToString();
            }
            string dadosDAVOS = " ";

            if (!string.IsNullOrEmpty(dadosDAV.osnrfabricacao))
            {
                dadosDAVOS = "PRODUTO OBJETO DO CONSERTO " + Environment.NewLine + Environment.NewLine +
                             "NR. FABRICAÇÃO:" + dadosDAV.osnrfabricacao + Environment.NewLine + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(dadosDAV.placa))
            {
                dadosDAVOS += "PRODUTO OBJETO DO CONSERTO" + Environment.NewLine + Environment.NewLine +
                    "Marca  : " + dadosDAV.marca + " Modelo: " + dadosDAV.modelo + Environment.NewLine +
                    "Ano    : " + dadosDAV.osnrfabricacao + " PLACA : " + dadosDAV.placa + Environment.NewLine +
                    "RENAVAM: " + dadosDAV.renavam;
            }

            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                infoEntrega += Environment.NewLine + Configuracoes.textoGarantia;

            vendadavBindingSource.DataSource = dados;
            caixadavBindingSource.DataSource = pagamentos;
            string descricaoTitulo = "DOCUMENTO AUXILIAR DE VENDA ORÇAMENTO";
            if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                descricaoTitulo = "ORÇAMENTO ORDEM DE SERVIÇO DAV-OS";

            ReportParameter titulo = new ReportParameter("lblTitulo", descricaoTitulo);
            ReportParameter empresa = new ReportParameter("lblEmpresa", Configuracoes.razaoSocial);
            ReportParameter dadosEmpresa = new ReportParameter("lblDadosEmpresa", Funcoes.FormatarCNPJ(Configuracoes.cnpj) +
                                            Environment.NewLine + Configuracoes.endereco +
                                            Environment.NewLine + Configuracoes.bairro + " " + Configuracoes.cidade + " " + Configuracoes.estado +
                                            Environment.NewLine + "Tel:" + Funcoes.FormatarTelefone(Configuracoes.telefone));
            ReportParameter idCliente = new ReportParameter("lblCliente", dadosDAV.codigocliente + " - " + dadosDAV.cliente);
            ReportParameter dadosCliente = new ReportParameter("lblDadosCliente", dadosIdcliente);

            ReportParameter davNumero = new ReportParameter("lblDAV",Funcoes.FormatarZerosEsquerda(numeroDAV, 10, false));
            ReportParameter totalBruto = new ReportParameter("lblTotalBruto", (dadosDAV.valor + dadosDAV.desconto).ToString());
            ReportParameter totalDesconto = new ReportParameter("lblTotalDesconto", (dadosDAV.desconto).ToString());
            ReportParameter encargos = new ReportParameter("lblTotalEncargos", (dadosDAV.encargos).ToString());
            ReportParameter totalLiquido = new ReportParameter("lblTotal", (dadosDAV.valor + dadosDAV.encargos).ToString());
            ReportParameter obs = new ReportParameter("lblObs", dadosDAV.observacao);
            ReportParameter servicos = new ReportParameter("lblServicos", infoServico);
            ReportParameter entrega = new ReportParameter("lblEnderecoEntrega", infoEntrega);
            ReportParameter davOS = new ReportParameter("davOS", dadosDAVOS);


            var dadosVendedor = "";
            if (dadosDAV.vendedor != "000")
                dadosVendedor = (from n in entidade.vendedores where n.codigo == dadosDAV.vendedor select n.codigo + " " + n.nome).First().ToString();
            ReportParameter vendedor = new ReportParameter("lblVendedor", dadosVendedor ?? "");
            ReportParameter operador = new ReportParameter("lblOperador", "OPER: " + GlbVariaveis.glb_Usuario);
            rptDAV.LocalReport.SetParameters(new ReportParameter[] { titulo, empresa, dadosEmpresa, idCliente, dadosCliente, davNumero, totalBruto, totalDesconto, encargos, totalLiquido, operador, obs, servicos, entrega, vendedor, davOS });
            this.rptDAV.RefreshReport();
            //reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);            
        }
        private void btnSair_Click(object sender, EventArgs e)
        {            
            Close();
        }
        private void rptDAV_RenderingComplete(object sender, RenderingCompleteEventArgs e)
        {
            this.rptDAV.PrintDialog();
        }
    }
}
