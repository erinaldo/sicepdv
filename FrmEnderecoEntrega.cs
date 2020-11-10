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
    public partial class FrmEnderecoEntrega : Form
    {
        public FrmEnderecoEntrega()
        {
            InitializeComponent();
            txtHoraEntrega.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtCodigo.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            //txtCidade.KeyPress += (objeto, evento) =>
            //    {
            //        if (evento.KeyChar == (char)Keys.Delete || evento.KeyChar == (char)Keys.Back)
            //        {
            //            evento.KeyChar = Convert.ToChar(0);
            //        };
            //    };
            txtCodigo.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.F3)
                    {
                        MostrarClientes();
                    }

                    if (evento.KeyCode == Keys.Escape)
                    {
                        this.Close();
                    }
                };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigo.Text))
                txtCodigo.Text = "0";

            if (txtCodigo.Text != "0")
            {                
                try
                {
                    Clientes procura = new Clientes();
                    if (procura.Procura(txtCodigo.Text))
                    {
                        txtRecebedor.Text = procura.nome;
                        txtEndereco.Text = procura.endereco;
                        txtNumero.Text = procura.numero;
                        txtBairro.Text = procura.bairro;
                        txtCEP.Text = procura.cep;
                        txtEstado.Text = procura.estado;
                        txtCidade.DataSource = Funcoes.RetonaCidade(txtEstado.Text);
                        txtCidade.Text = procura.cidade;
                       
                    }
                    else
                        MessageBox.Show("Cliente não foi encontrado", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                catch (Exception)
                {

                    throw;
                }
            };
            
        }

        private void FrmEnderecoEntrega_Load(object sender, EventArgs e)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            txtData.Value = GlbVariaveis.Sys_Data;
            txtHoraEntrega.Text = "00:08";
            txtEstado.Text = (from n in entidade.filiais
                              where n.CodigoFilial == GlbVariaveis.glb_filial
                              select n.estado).First();

            txtCidade.Text = (from n in entidade.filiais
                              where n.CodigoFilial == GlbVariaveis.glb_filial
                              select n.cidade).FirstOrDefault();

            
            if (!string.IsNullOrEmpty(Venda.dadosEntrega.recebedor))
            {
                txtRecebedor.Text = Venda.dadosEntrega.recebedor;
                txtEndereco.Text = Venda.dadosEntrega.endereco;
                txtNumero.Text = Venda.dadosEntrega.numero;
                txtCEP.Text = Venda.dadosEntrega.cep;
                txtCidade.Text = Venda.dadosEntrega.cidade;
                txtBairro.Text = Venda.dadosEntrega.bairro;
                txtEstado.Text = Venda.dadosEntrega.estado;
                txtHoraEntrega.Text = string.Format("{0:HH:mm}", Venda.dadosEntrega.hora);
                txtData.Value = Venda.dadosEntrega.data;
            };


        }

        private void FrmEnderecoEntrega_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.F7)
                FuncoesPAFECF.ChamarMenuFiscal();
        }


        private void txtCidade_Enter(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCidade.Text))
            txtCidade.DataSource = Funcoes.RetonaCidade(txtEstado.Text);
        }

        private void txtCidade_Leave(object sender, EventArgs e)
        {
            txtCidade.Text = txtCidade.Text.ToUpper();
            if (!txtCidade.Items.Contains(txtCidade.Text))
            {
                MessageBox.Show("Cidade não encontrada", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCidade.Focus();
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {

            if (txtRecebedor.Text.Trim() == "")
            {
                MessageBox.Show("Campos Recebedor não poder ser vazio!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if(txtEndereco.Text.Trim() == "")
            {
                MessageBox.Show("Campos Endereço não poder ser vazio!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtNumero.Text.Trim() == "")
            {
                MessageBox.Show("Campos Numero não poder ser vazio!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtBairro.Text.Trim() == "")
            {
                MessageBox.Show("Campos Bairro não poder ser vazio!", "Anteção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtCidade.Text = txtCidade.Text.ToUpper();
            if (!txtCidade.Items.Contains(txtCidade.Text))
            {
                MessageBox.Show("Cidade não encontrada", "SICEpdv", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(txtHoraEntrega.Text))
                txtHoraEntrega.Text = "00:00";

            var dadosEntrega = new DadosEntrega();
            dadosEntrega.recebedor = txtRecebedor.Text;
            dadosEntrega.endereco = txtEndereco.Text;
            dadosEntrega.numero = txtNumero.Text;
            dadosEntrega.cep = txtCEP.Text;
            dadosEntrega.cidade = txtCidade.Text;
            dadosEntrega.bairro = txtBairro.Text;
            dadosEntrega.estado = txtEstado.Text;
            dadosEntrega.hora = Convert.ToDateTime(string.Format("{0:HH:mm}", txtHoraEntrega.Text));
            dadosEntrega.data = txtData.Value;
            dadosEntrega.observacao = txtObs.Text+Environment.NewLine;
            
            

            Venda.dadosEntrega = dadosEntrega;
            this.Close();
        }

        private void txtEstado_SelectedValueChanged(object sender, EventArgs e)
        {
            txtCidade.DataSource = Funcoes.RetonaCidade(txtEstado.Text);
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void MostrarClientes()
        {
            FrmClientes frmCli = new FrmClientes();
            frmCli.ShowDialog();
            txtCodigo.Text = Convert.ToString(FrmClientes.codigoCliente);
            txtCodigo.Focus();
        }

        private void btnProcurar_Click_1(object sender, EventArgs e)
        {
            MostrarClientes();
        }

        private void txtBairro_Leave(object sender, EventArgs e)
        {
            if (txtBairro.Text.Length < 5)
            {
                MessageBox.Show("Bairro inválido!", "Atenção", MessageBoxButtons.OK);
                txtBairro.Focus();
            }
        }
    }
}
