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
    public partial class FrmClientes : Form
    {
       
        public static int codigoCliente = 0;
        public FrmClientes()
        {
            
            InitializeComponent();
            

            txtProcura.KeyUp += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.F7)
                    {
                        ChamarMenuFiscal();
                    }
                    if (evento.KeyCode == Keys.Up || evento.KeyCode == Keys.Down)
                    {
                        dtgClientes.Focus();
                        evento.SuppressKeyPress = true;
                    }

                    if (evento.KeyCode == Keys.Escape)
                        Sair();

                    if (evento.KeyCode == Keys.Return && txtProcura.Text.Length>0 )
                        Sair();
                      if (chkPesquisaAut.Checked)
                        Procurar();                        
                };
            dtgClientes.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Return && dtgClientes.Rows.Count > 0 )
                        Sair();


                    if (evento.KeyCode !=  Keys.Up && evento.KeyCode != Keys.Down && evento.KeyCode != Keys.F7)
                    {
                        txtProcura.Text = "";
                        txtProcura.Text = evento.KeyData.ToString();
                        SendKeys.Send("{Right}");
                        txtProcura.Focus();
                    }
                };
            dtgClientes.CellPainting += (objeto, evento) => MostrarSaldo();
        }

        private static void ChamarMenuFiscal()
        {
            FuncoesPAFECF.ChamarMenuFiscal();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Sair();
        }

        private void Sair()
        {
            codigoCliente = 0;
            if (dtgClientes.RowCount > 0)
                codigoCliente = Convert.ToInt32("0" + dtgClientes.CurrentRow.Cells["codigo"].Value);
            this.Close();
        }

        private void Procurar(int limit = 100)
        {
            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2 && Conexao.ConexaoOnline() == false)
                entidade = Conexao.CriarEntidade(false);

            Int32 codigo = 0;
            if (rdbCodigo.Checked || rdbCodigo.Checked)
            {                                
                if (!int.TryParse(txtProcura.Text,out codigo))
                return;
            };

            if (rdbNome.Checked)
            {
                /*dtgClientes.DataSource = (from n in entidade.clientes
                                         where n.Nome.StartsWith(txtProcura.Text) 
                                         && n.ativo=="S"
                                         orderby n.Nome
                                         select new
                                         {
                                             n.CodigoFilial,
                                             n.Codigo,
                                             n.Nome,
                                             n.apelido,
                                             n.ativo,
                                             n.sexo,
                                             n.observacao,
                                             n.situacao,
                                             n.cidade,
                                             n.endereco,
                                             n.debito,
                                             n.debitoch,
                                             n.credito,
                                             n.saldo,
                                             saldoch = n.saldo - n.debitoch
                                         }).Take(limit); */


                string sql = "SELECT codigoFilial,codigo,Nome,apelido,ativo,sexo,observacao,situacao,cidade,endereco,IFNULL(debito,0) AS debito,IFNULL(debitoch,0) AS debitoch,IFNULL(credito,0) AS credito,IFNULL(saldo,0) AS saldo,(IFNULL(saldo,0)-IFNULL(debitoch,0)) AS saldoch FROM clientes WHERE clienteCrediario = 'S' and ativo = 'S' and Nome like '" + txtProcura.Text+"%' LIMIT "+ limit;
                dtgClientes.DataSource = Conexao.CriarEntidade().ExecuteStoreQuery<clientesModel>(sql).ToList();


            }


            if (rdbApelido.Checked)
            {
                /*dtgClientes.DataSource = (from n in entidade.clientes
                                         where n.apelido.StartsWith(txtProcura.Text)
                                         && n.ativo == "S"
                                         orderby n.Nome
                                         select new
                                         {
                                             n.CodigoFilial,
                                             n.Codigo,
                                             n.Nome,
                                             n.apelido,
                                             n.ativo,
                                             n.sexo,
                                             n.observacao,
                                             n.situacao,
                                             n.cidade,
                                             n.endereco,
                                             n.debito,
                                             n.debitoch,
                                             n.credito,
                                             n.saldo,
                                             saldoch = n.saldo-n.debitoch

                                         }).Take(100); */


                string sql = "SELECT codigoFilial,codigo,Nome,apelido,ativo,sexo,observacao,situacao,cidade,endereco,IFNULL(debito,0) AS debito,IFNULL(debitoch,0) AS debitoch,IFNULL(credito,0) AS credito,IFNULL(saldo,0) AS saldo,(IFNULL(saldo,0)-IFNULL(debitoch,0)) AS saldoch FROM clientes WHERE clienteCrediario = 'S' and ativo = 'S' and apelido like '" + txtProcura.Text + "%' LIMIT " + limit;
                dtgClientes.DataSource = Conexao.CriarEntidade().ExecuteStoreQuery<clientesModel>(sql).ToList();
            }


            if (rdbCodigo.Checked)
            {
                /*dtgClientes.DataSource = from n in entidade.clientes
                                         where n.Codigo == codigo
                                         && n.ativo == "S"
                                         orderby n.Nome
                                         select new
                                         {
                                             n.CodigoFilial,
                                             n.Codigo,
                                             n.Nome,
                                             n.apelido,
                                             n.ativo,
                                             n.sexo,
                                             n.observacao,
                                             n.situacao,
                                             n.cidade,
                                             n.endereco,
                                             n.debito,
                                             n.debitoch,
                                             n.credito,
                                             n.saldo,
                                             saldoch = n.saldo - n.debitoch
                                         };  */


                string sql = "SELECT codigoFilial,codigo,Nome,apelido,ativo,sexo,observacao,situacao,cidade,endereco,IFNULL(debito,0) AS debito,IFNULL(debitoch,0) AS debitoch,IFNULL(credito,0) AS credito,IFNULL(saldo,0) AS saldo,(IFNULL(saldo,0)-IFNULL(debitoch,0)) AS saldoch FROM clientes WHERE clienteCrediario = 'S' and ativo = 'S' and Codigo = '" + txtProcura.Text + "' LIMIT " + limit;
                dtgClientes.DataSource = Conexao.CriarEntidade().ExecuteStoreQuery<clientesModel>(sql).ToList();
            }

            if (rdbCPF.Checked)
            {
                /*dtgClientes.DataSource = (from n in entidade.clientes
                                         where n.cpf.StartsWith(txtProcura.Text) || n.cnpj.StartsWith(txtProcura.Text)
                                         && n.ativo == "S"
                                         orderby n.Nome
                                         select new
                                         {
                                             n.CodigoFilial,
                                             n.Codigo,
                                             n.Nome,
                                             n.apelido,
                                             n.ativo,
                                             n.sexo,
                                             n.observacao,
                                             n.situacao,
                                             n.cidade,
                                             n.endereco,
                                             n.debito,
                                             n.debitoch,
                                             n.credito,
                                             n.saldo,
                                             saldoch = n.saldo - n.debitoch
                                         }).Take(100); */

                string sql = "SELECT codigoFilial,codigo,Nome,apelido,ativo,sexo,observacao,situacao,cidade,endereco,IFNULL(debito,0) AS debito,IFNULL(debitoch,0) AS debitoch,IFNULL(credito,0) AS credito,IFNULL(saldo,0) AS saldo,(IFNULL(saldo,0)-IFNULL(debitoch,0)) AS saldoch  FROM clientes WHERE clienteCrediario = 'S' and ativo = 'S' and (cpf like '" + txtProcura.Text + "%' or cnpj like '" + txtProcura.Text + "%') LIMIT " + limit;
                dtgClientes.DataSource = Conexao.CriarEntidade().ExecuteStoreQuery<clientesModel>(sql).ToList();
            }          

        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            chkPesquisaAut.Checked = Configuracoes.procuraAutomaticaCli;
            Procurar(1);
            txtProcura.Focus();
        }

        private void btnVer_Click(object sender, EventArgs e)
        {
            if (dtgClientes.RowCount > 0)
            codigoCliente = Convert.ToInt32("0" + dtgClientes.CurrentRow.Cells["codigo"].Value);
            FrmFotoCliente foto = new FrmFotoCliente();
            FrmFotoCliente.codCli = codigoCliente;
            foto.ShowDialog();
        }

        void MostrarSaldo()
        {
            if (dtgClientes.RowCount != 0)
            {
                lblSaldo.ForeColor = Color.Blue;
                lblSaldoCH.ForeColor = Color.Blue;
                lblCredito.Text = string.Format("{0:c2}", dtgClientes.CurrentRow.Cells["credito"].Value);
                lblDebito.Text = string.Format("{0:c2}", dtgClientes.CurrentRow.Cells["debito"].Value); 
                lblSaldo.Text = string.Format("{0:c2}", dtgClientes.CurrentRow.Cells["saldo"].Value);

                lblDebitoCH.Text = string.Format("{0:c2}", dtgClientes.CurrentRow.Cells["debitoch"].Value);
                lblSaldoCH.Text = string.Format("{0:c2}", dtgClientes.CurrentRow.Cells["saldoch"].Value);

                if (Convert.ToDecimal(dtgClientes.CurrentRow.Cells["saldoch"].Value) < 0)
                {
                    lblSaldoCH.ForeColor = Color.Red;
                }
                if (Convert.ToDecimal(dtgClientes.CurrentRow.Cells["saldo"].Value) < 0)
                {
                    lblSaldo.ForeColor = Color.Red;
                }

            }
            else
            {
                lblDebito.Text = "0.00";
                lblCredito.Text = "0.00";
                lblSaldo.Text = "0.00";
                lblDebitoCH.Text = "0.00";
                lblSaldoCH.Text = "0.00";
            }
        }

        private void rdbCodigo_CheckedChanged(object sender, EventArgs e)
        {
            txtProcura.Focus();
        }

        private void rdbApelido_CheckedChanged(object sender, EventArgs e)
        {
            txtProcura.Focus();
        }

        private void rdbNome_CheckedChanged(object sender, EventArgs e)
        {
            txtProcura.Focus();
        }

        private void rdbCPF_CheckedChanged(object sender, EventArgs e)
        {
            txtProcura.Focus();
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            Procurar();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("osk.exe");
            txtProcura.Focus();
        }

        private void dtgClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int codigo = Convert.ToInt32(dtgClientes.CurrentRow.Cells["codigo"].Value);
                string nome = Convert.ToString(dtgClientes.CurrentRow.Cells["nome"].Value);
                string drive = GlbVariaveis.glb_chaveIQCard + codigo.ToString();
                ArmazenamentoCloud cloud = new ArmazenamentoCloud();
                ArmazenamentoCloud.idCliente = codigo;
                ArmazenamentoCloud.nome = nome;
                ArmazenamentoCloud.drive = drive;
                cloud.ShowDialog();
            };

            if (e.ColumnIndex == 1)
            {
                AdicionarIQCARDCliente.idCliente = Convert.ToInt32(dtgClientes.CurrentRow.Cells["codigo"].Value);
                AdicionarIQCARDCliente fidelidade = new AdicionarIQCARDCliente();
                fidelidade.ShowDialog();
            }
        }

        private void btnProcurar_Click_1(object sender, EventArgs e)
        {
            Procurar();
        }
    }
}
