using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class FrmVendedores : Form
    {
        UcVendedores mostraVendedores = new UcVendedores();   
        
        public FrmVendedores()
        {
            InitializeComponent();

            this.KeyDown += (objeto, evento) =>
            {
                if (evento.KeyCode == Keys.Escape)
                    this.Close();
            };
            
            mostraVendedores.Click += (objeto, evento) => this.Close();
        }

        private void FrmVendedores_Load(object sender, EventArgs e)
        {            
            this.Controls.Add(mostraVendedores);
            this.mostraVendedores.clickBotao += new UcVendedores.ClicarNoBotao(Fechar);
            this.Size = new Size(mostraVendedores.Size.Width+5,mostraVendedores.Size.Height+5);
        }

        void Fechar(object sender)
        {
            this.Close();
        }

        private void FrmVendedores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void bntEscolherVendedor_Click(object sender, EventArgs e)
        {
            procurarvendedor();

            Vendedor.VendaVendedor(Vendedor.codigoVendedor);

            if (Vendedor.codigoVendedor != null && Vendedor.codigoVendedor != "0" && Vendedor.codigoVendedor != "")
                this.Close();
            else
            {
                MessageBox.Show("Codigo do vendedor inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Vendedor.codigoVendedor = "000";
                Vendedor.nomeVendedor = "Geral";
                this.Close();
            }
        }

        private void procurarvendedor()
        {
            if (txtCodigo.Text.Trim() != "")
            {
                siceEntities entidade;
                if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                    entidade = Conexao.CriarEntidade(false);
                else
                    entidade = Conexao.CriarEntidade();

                var vendedores = from n in entidade.vendedores
                                 where n.CodigoFilial == GlbVariaveis.glb_filial && n.codigo == txtCodigo.Text
                                 && n.ativo == "S"
                                 orderby n.nome
                                 select new { n.codigo, n.nome };

                if (vendedores.FirstOrDefault() != null)
                {
                    Vendedor.codigoVendedor = vendedores.FirstOrDefault().codigo;
                    Vendedor.nomeVendedor = vendedores.FirstOrDefault().nome;
                    Vendedor.VendaVendedor(Vendedor.codigoVendedor);
                }
                else
                {
                    Vendedor.codigoVendedor = "000";
                    Vendedor.nomeVendedor = "Geral";
                }

                
            }

           
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            procurarvendedor();
            //bntEscolherVendedor_Click(sender, e);
        }

        private void txtCodigo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                procurarvendedor();

                if (Vendedor.codigoVendedor != null && Vendedor.codigoVendedor != "0" && Vendedor.codigoVendedor != "")
                    this.Close();
                else
                    MessageBox.Show("Codigo do vendedor inválido", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
