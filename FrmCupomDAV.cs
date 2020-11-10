using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace SICEpdv
{
    public partial class FrmCupomDAV : Form
    {
        int numeroDAV;

        public FrmCupomDAV(int numero )
        {
            numeroDAV = numero;
            InitializeComponent();
        }

        private void FrmCupomDAV_Load(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            var dadosDAV = (from n in entidade.contdav
                           where n.numero == numeroDAV
                           select new
                           {
                               codigo = n.codigocliente,
                               cliente = n.cliente,
                               cpf = n.ecfCPFCNPJconsumidor,
                               vendedor = n.vendedor                               
                           }).First();
            

            var dados = from n in entidade.vendadav
                        where n.documento == numeroDAV
                        && n.cancelado == "N"
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
                        };
            var pagamentos = from n in entidade.caixadav
                             where n.documento == numeroDAV
                             select new
                             {
                                 Nrparcela = n.Nrparcela,
                                 tipopagamento= n.tipopagamento,
                                 //vencimento= n.vencimento,
                                 valor = n.valor,                                 
                             };

            CupomDAV cupom = new CupomDAV();
            cupom.Subreports["pagamentos"].SetDataSource(pagamentos.ToList());
            cupom.OpenSubreport("pagamentos");
            cupom.SetDataSource(dados.ToList());
            
            TextObject txtDAV = (TextObject)cupom.ReportDefinition.ReportObjects["txtDAV"];
            TextObject txtCodBarra = (TextObject)cupom.ReportDefinition.ReportObjects["txtCodBarras"];
            TextObject txtCliente = (TextObject)cupom.ReportDefinition.ReportObjects["txtCliente"];
            TextObject txtVendedor = (TextObject)cupom.ReportDefinition.ReportObjects["txtVEndedor"];
            TextObject txtOperador = (TextObject)cupom.ReportDefinition.ReportObjects["txtOperador"];
            // Dados Empresa

            TextObject txtEmpresa = (TextObject)cupom.ReportDefinition.ReportObjects["txtEmpresa"];
            TextObject txtCnpj = (TextObject)cupom.ReportDefinition.ReportObjects["txtCnpj"];
            TextObject txtEndereco = (TextObject)cupom.ReportDefinition.ReportObjects["txtEndereco"];
            TextObject txtcidade = (TextObject)cupom.ReportDefinition.ReportObjects["txtCidade"];

            TextObject txtTelefone = (TextObject)cupom.ReportDefinition.ReportObjects["txtTelefone"];
            txtEmpresa.Text = Configuracoes.razaoSocial;
            txtCnpj.Text ="CNPJ : " + Funcoes.FormatarCNPJ(Configuracoes.cnpj);
            txtEndereco.Text = Configuracoes.endereco;
            txtcidade.Text = Configuracoes.cidade + " " + Configuracoes.bairro + " " + Configuracoes.estado;
            txtTelefone.Text = Funcoes.FormatarTelefone(Configuracoes.telefone);            

            txtDAV.Text = Funcoes.FormatarZerosEsquerda(numeroDAV,10,false);
            txtCodBarra.Text = "";// numeroDAV.ToString();
            txtCliente.Text =  dadosDAV.codigo + " " + dadosDAV.cliente + "  CNPJ/CPF: " + dadosDAV.cpf;
            txtOperador.Text = GlbVariaveis.glb_Usuario;
            txtVendedor.Text = Venda.vendedor;

            if (dadosDAV.vendedor!="000")txtVendedor.Text = (from n in entidade.vendedores where n.codigo == dadosDAV.vendedor select n.codigo+" "+n.nome).First().ToString();            

            crvCupom.ReportSource = cupom;
        }

    }
}
