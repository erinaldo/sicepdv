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
    public partial class frmPreVenda : Form
    {

        TeclaNumerico teclado = new TeclaNumerico(Color.White);
        private string controle = "txtNumero";
        private string tecla = "";
        private bool mesclar = false;
        private bool mesclarItens = false;
        private bool editarDAV = false;
        List<Int32> mescladas = new List<int>();
        int idCliente = 0;
        decimal desconto = 0;
        decimal encargos = 0;
        decimal total = 0;
        bool Mostrarprevenda = true;
        bool MostrarDAV = true;
        public frmPreVenda(bool prevenda,bool DAV)
        {
            Mostrarprevenda = prevenda;
            MostrarDAV = DAV;
           

            InitializeComponent();
            pnlTeclado.Controls.Add(teclado);

            if (!DAV)
            {
                btnMesclarItem.Visible = false;
                bntEditarDAV.Visible = false;
            }

            txtNumero.KeyPress += (objeto, evento) => Funcoes.DigitarNumerosPositivos(objeto, evento);
            txtNumero.Enter += (objeto, evento) => controle = ActiveControl.Name;
            txtNumero.KeyDown += (objeto, evento) =>
                {
                    if (evento.KeyCode == Keys.Enter)
                        Processar(Convert.ToInt32(txtNumero.Text));

                    if (evento.KeyCode == Keys.F7)
                    {
                        ChamarMenuFiscal();
                    }
                };

            
            this.teclado.clickBotao += new TeclaNumerico.ClicarNoBotao(DelegateTeclado);

            if (ConfiguracoesECF.pdv == true)
            {
                bntEditarDAV.Visible = false;
                editarDAV = false;
            }
        }

        void DelegateTeclado(object sender, string text)
        {             
            tecla = text;
            switch (tecla)
            {
                case "X":
                    break;
                case "Enter":
                    TeclaEnter();
                    break;
                case "Limpar":
                    Control[] ctls = this.Controls.Find(controle, true);
                    if (ctls[0] is TextBox)
                    {
                        TextBox txtBox = ctls[0] as TextBox;
                        txtBox.Text = "";
                        txtBox.Focus();
                    };
                    break;
                default:
                    PreencheCampo();
                    break;
            }
        }

        private void TeclaEnter()
        {
            switch (controle)
            {
                case "txtNumero":
                    Processar(Convert.ToInt32(txtNumero.Text));
                    break;
                default:
                    if (controle == "") return;
                    Control[] ctls = this.Controls.Find(controle, true);
                    ctls[0].Focus();
                    SendKeys.Send("{TAB}");
                    break;
            }
        }

        private void PreencheCampo()
        {
            if (controle == "") return;
            Control[] ctls = this.Controls.Find(controle, true);
            TextBox txtBox = ctls[0] as TextBox;
            if (txtBox.Enabled == false)
                return;
            txtBox.Text += this.tecla;
            txtBox.Focus();
        }

        private void Construir()
        {
            List<StructPrevendaDAV> lstPrevendaDAV = new List<StructPrevendaDAV>();
            int x = 0;
            int yaltura = 0;

            siceEntities entidade = Conexao.CriarEntidade();            
            if (Mostrarprevenda)
            {
                var dados = (from n in entidade.contprevendaspaf
                            orderby n.numero descending
                            where n.finalizada == "N" && n.cancelada == "N"
                            && n.codigofilial == GlbVariaveis.glb_filial
                            select new
                            {
                                numero = n.numeroDAVFilial,
                                valor = n.valor,
                                cliente = n.cliente
                            }).Take(30);
                foreach (var item in dados)
                {
                    StructPrevendaDAV numero;
                    numero.numero= item.numero;
                    numero.valor = item.valor;
                    numero.cliente = item.cliente;
                    numero.atacado = false;
                    numero.origem = "SICEpdv";

                    lstPrevendaDAV.Add(numero);
                }

            };
            
            if (MostrarDAV)
            {

                if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                {
                    var dados = (from n in entidade.contdav
                                 orderby n.numeroDAVFilial descending
                                 where n.finalizada == "N" && n.cancelada == "N"
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select new
                                 {
                                     numero = n.numeroDAVFilial,
                                     valor = n.valor,
                                     cliente = n.cliente
                                 }).Take(30);

                    foreach (var item in dados)
                    {
                        var dadosDAV = Conexao.CriarEntidade().ExecuteStoreQuery<dadosDAV>("select ifnull(vendaatacado,'N') as vendaatacado, ifnull(origem,'SICEpdv') as origem, statusPagamento, formapagamento, idTransacaogateway   from contdav where numeroDAVFilial = '" + item.numero+"'  and codigofilial = '"+GlbVariaveis.glb_filial+"'").FirstOrDefault();
                        //string origem = Conexao.CriarEntidade().ExecuteStoreQuery<string>("select ifnull(origem,'SICEpdv') from contdav where numeroDAVFilial = '" + item.numero + "'  and codigofilial = '" + GlbVariaveis.glb_filial + "'").FirstOrDefault();

                        StructPrevendaDAV numero;
                        numero.numero = item.numero;
                        numero.valor = item.valor;
                        numero.cliente = item.cliente;
                        numero.origem = dadosDAV.origem;

                        if (dadosDAV.vendaatacado == "S")
                            numero.atacado = true;
                        else
                            numero.atacado = false;

                        lstPrevendaDAV.Add(numero);
                    }
                }

                if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                {
                    var dados = (from n in entidade.contdavos
                                 orderby n.numero descending
                                 where n.finalizada == "N" && n.cancelada == "N"
                                 && n.codigofilial == GlbVariaveis.glb_filial
                                 select new
                                 {
                                     numero = n.numeroDAVFilial,
                                     valor = n.valor,
                                     cliente = n.cliente
                                 }).Take(30);
                    foreach (var item in dados)
                    {
                        StructPrevendaDAV numero;
                        numero.numero = item.numero;
                        numero.valor = item.valor;
                        numero.cliente = item.cliente;
                        numero.atacado = false;
                        numero.origem = "SICEpdv";
                        lstPrevendaDAV.Add(numero);
                    }
                }

            };
            

            var txt = "";
            foreach (var item in lstPrevendaDAV)                
            {                
                if (x > 3)
                {
                    x = 0;
                    yaltura = yaltura + 60;
                }

                string valor = item.valor.ToString("N2").Length < 7 ? "R$(" + item.valor.ToString("N2") + ")" : item.valor.ToString("N2");

                Button btnPreVenda = new Button();
                btnPreVenda.Name = item.numero.ToString();
                btnPreVenda.Font = new Font("Verdana", 9);
                btnPreVenda.FlatStyle = FlatStyle.Flat;
                btnPreVenda.FlatAppearance.BorderSize = 1;
                txt = item.numero.ToString();
                btnPreVenda.Text = FormatarPreVenda(item.numero.ToString() + "\n" + valor);
                btnPreVenda.Size = new Size(93, 60);
                btnPreVenda.Location = new System.Drawing.Point(x * 93,yaltura);

                if (item.atacado == true)
                    btnPreVenda.BackColor = Color.Yellow;

                if(item.origem == "iqShop")
                    btnPreVenda.BackColor = Color.LightGreen;


                btnPreVenda.Click += (objeto, evento) =>
                    {
                        txtNumero.Text = btnPreVenda.Name.ToString();
                        Processar(Convert.ToInt32(btnPreVenda.Name.ToString()));
                        btnPreVenda.ForeColor = Color.Red;
                    };
                pnlPreVenda.Controls.Add(btnPreVenda);
                x++;
            }
        }

        private string FormatarPreVenda(string texto)
        {
            int tamanho = texto.Length;            
            switch (tamanho)
            {
                case 5:
                   texto = texto.Substring(0, 2) + " " + texto.Substring(2, 3);
                   return texto;
                case 6:
                   texto = texto.Substring(0, 3)+ " "+texto.Substring(3,3);
                   return texto;                    
                default:
                    return texto;            
            }            
            
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PreVenda_Load(object sender, EventArgs e)
        {
            txtNumero.Focus();
            if (Mostrarprevenda == true)
            {
                btnMesclar.Visible = false;
                chkManter.Visible = false;
            }

            // Aqui verifica se existem pedido feitos no IQCARD.
            // Suspenso para nao haver lag de espera por que a maioria naousa o aPP STore sotre
            //try
            //{
            //    IqCard iqcard = new IqCard();
            //    int contadorPedido = 0;
            //    if (Funcoes.VerificarConexaoInternet() && Configuracoes.coefecientePontosIQCard > 0 && !String.IsNullOrEmpty(GlbVariaveis.glb_chaveIQCard) && (DateTime.Now.Subtract(IqCard.horaVerificadoPedido).TotalMinutes > 30))
            //    {                    
            //        contadorPedido = iqcard.ContadorPedido("solicitado");
            //        btnAppStore.Text = contadorPedido.ToString()+ " PEDIDOS NO APP STORE";
            //    }

            //    if (contadorPedido == 0)
            //    {
            //        //pnlPedido.Visible = false;                    
            //    }
            //}
            //catch (Exception)
            //{

            //}

            Construir();
        }

        private void Processar(int numero)
        {

            int contadorItens = (from n in Conexao.CriarEntidade().vendas
                                 where n.id == GlbVariaveis.glb_IP
                                 select n.id).Count();

            if (contadorItens > 0)
            {
                MessageBox.Show("Existem itens na venda atual, por favor fechar o módulo de DAV e termine a transação");
                return;
            }

            PreVenda prevenda = new PreVenda();
            #region Pre-venda
            if (Mostrarprevenda)
            {
                var retorno = prevenda.RetornaPreVenda(numero);

                if (retorno.Count() == 0)
                {
                    MessageBox.Show("Número não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                foreach (var item in retorno)
                {
                    if (item.finalizada == "S")
                    {
                        MessageBox.Show("Pré-venda já foi encerrada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (item.cancelada == "S")
                    {
                        MessageBox.Show("Pré-venda cancelada", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (mesclar == true)
                    {                       
                        if (!mescladas.Contains(numero))
                        {
                            idCliente = item.codigocliente;                            
                            desconto += item.desconto;
                            encargos += item.encargos;
                            total += item.valor;
                            mescladas.Add(numero);
                        }
                        return;
                    };

                    desconto = item.desconto;
                    encargos = item.encargos;
                    idCliente = item.codigocliente;
                    total = item.valor;

                }
            };
            #endregion
            #region DAV
            if (MostrarDAV)
            {
                if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Geral.ToString())
                {
                    var retorno = prevenda.RetornarDAV(numero);

                    if (retorno.Count() == 0)
                    {
                        MessageBox.Show("Número não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    foreach (var item in retorno)
                    {
                        if (item.finalizada == "S")
                        {
                            MessageBox.Show("DAV já foi encerrado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        if (item.cancelada == "S")
                        {
                            MessageBox.Show("DAV cancelado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        if (mesclar == true)
                        {
                            if (!mescladas.Contains(numero))
                            {
                                idCliente = item.codigocliente;
                                desconto += item.desconto;
                                encargos += item.encargos;
                                total += item.valor;
                                mescladas.Add(numero);
                                if (mesclarItens)
                                {
                                    MesclarItens frmMesclar = new MesclarItens(numero);
                                    frmMesclar.ShowDialog();

                                }
                            }
                            return;
                        };

                        desconto = item.desconto;
                        encargos = item.encargos;
                        idCliente = item.codigocliente;
                        total = item.valor;

                    }
                }

                if (GlbVariaveis.glb_TipoPAF == GlbVariaveis.tipoPAF.Serviço.ToString())
                {
                    var retorno = prevenda.RetornarDAVOS(numero);

                    if (retorno.Count() == 0)
                    {
                        MessageBox.Show("Número não encontrado.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    foreach (var item in retorno)
                    {
                        if (item.finalizada == "S")
                        {
                            MessageBox.Show("DAV já foi encerrado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        if (item.cancelada == "S")
                        {
                            MessageBox.Show("DAV cancelado", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        if (mesclar == true)
                        {
                            if (!mescladas.Contains(numero))
                            {
                                idCliente = item.codigocliente;
                                desconto += item.desconto;
                                encargos += item.encargos;
                                total += item.valor;
                                mescladas.Add(numero);
                            }
                            return;
                        };

                        desconto = item.desconto;
                        encargos = item.encargos;
                        idCliente = item.codigocliente;
                        total = item.valor;

                    }
                }


                if ((ConfiguracoesECF.idECF == 0 || !ConfiguracoesECF.pdv || ConfiguracoesECF.davporImpressoraNaoFiscal) && editarDAV == false)
                {
                    if (ConfiguracoesECF.perfil == "Y")
                    {
                        return;
                    }

                    if (MessageBox.Show("Imprimir DAV ?", "SICEpdv", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (ConfiguracoesECF.davImpressaoCarta == true)
                        {
                            ImpressaoDAV cupom = new ImpressaoDAV(numero);
                            cupom.ShowDialog();
                            cupom.Dispose();
                        }
                        else
                        {
                            Venda objVenda = new Venda();
                            objVenda.ImprimirDAV(numero); 
                        }

                    };
                    
                    return;
                }

                
            };
            #endregion            

            if (editarDAV == true)
            {
                PreVenda.MontarDAV(numero);
                Venda.ApagarItensFormaPagamento("");
                Venda.ZerarNumeroDAVTransfTemp();
                this.Close();
                return;
            }

            IniciarFechamento(numero);           
        }

        private void IniciarFechamento(int numero)
        {
            if (Mostrarprevenda)
            {
                FrmFinalizarPreVenda processaPre = new FrmFinalizarPreVenda(numero, idCliente, total, desconto,encargos, mescladas,true,false,chkManter.Checked);
                processaPre.ShowDialog();
                processaPre.Dispose();
            };
            if (MostrarDAV)
            {
                FrmFinalizarPreVenda processaPre = new FrmFinalizarPreVenda(numero, idCliente, total, desconto, encargos, mescladas, false, true, chkManter.Checked);
                processaPre.ShowDialog();
                processaPre.Dispose();
            };

            if (_pdv.numeroPreVenda > 0 || _pdv.numeroDAV > 0)
            {
                this.Close();
                
            }
            desconto = 0;
            encargos = 0;
            total = 0;

        }

        private void frmPreVenda_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F7)
            {
                ChamarMenuFiscal();
            }
        }

        private void btnMesclar_Click(object sender, EventArgs e)
        {
            MesclarDAV();       
        }

        private void MesclarDAV()
        {

            int contadorItens = (from n in Conexao.CriarEntidade().vendas
                                 where n.id == GlbVariaveis.glb_IP
                                 select n.id).Count();

            if (contadorItens > 0)
            {
                MessageBox.Show("Existem itens na venda atual, por favor fechar o módulo de DAV e termine a transação");
                return;
            }

            if (!mesclar)
            {
                mescladas.Clear();
                mesclar = true;                
                btnMesclar.BackColor = Color.Green;
                chkManter.Visible = true;
                chkManter.Checked = true;
                editarDAV = false;
            }
            else
            {
                chkManter.Visible = false;
                chkManter.Checked = false;
                mesclar = false;
                editarDAV = false;
                mesclarItens = false;
                btnMesclar.BackColor = System.Drawing.SystemColors.ActiveCaption;
                foreach (var item in mescladas)
                {
                    Control[] btn = this.Controls.Find(item.ToString(), true);
                    btn[0].ForeColor = Color.Black;
                }
                mescladas.Clear();
            }

            desconto = 0;
            encargos = 0;
            total = 0;
            Venda.ApagarItensFormaPagamento("itenspagamentos");
        }

        private void btnFinalizarMescladas_Click(object sender, EventArgs e)
        {
            if (ConfiguracoesECF.idECF == 0 || !ConfiguracoesECF.pdv)
            {
                MessageBox.Show("Só é possível finalizar no terminal tipo PDV");
                return;
            }

            if (mescladas.Count == 0)
                return;

            if (mesclar && mescladas.Count <= 1)
            {
                MessageBox.Show("Escolha ao menos 2 DAvs para haver mesclagem.");
                return;
            }

            IniciarFechamento(0);
        }

        private struct StructPrevendaDAV
        {
            public long numero;
            public decimal valor;
            public string cliente;
            public bool atacado;
            public string origem;
        }

        private void frmPreVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Habite a propriedadeKeypreview = true com isso 
            // eliminar-se o som quando tecla enter e pula para o próximo campo
            if (e.KeyChar == 13)
            {
                this.ProcessTabKey(true);
                e.Handled = true;
            }

            if (e.KeyChar == Convert.ToChar(Keys.F7))
                ChamarMenuFiscal();

        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            string tipo = "DAV";
            if (Mostrarprevenda)
                tipo = "PRE";

            FrmConsultarDAVPrevenda frmConsultar = new FrmConsultarDAVPrevenda(tipo);
            frmConsultar.ShowDialog();
        }

        private static void ChamarMenuFiscal()
        {
            FrmMenuFiscal frmFiscal = new FrmMenuFiscal();
            frmFiscal.ShowDialog();
        }

        private void btnMesclarItem_Click(object sender, EventArgs e)
        {
            MesclarDAV();
            mesclarItens = true;
        }

        private void bntEditarDAV_Click(object sender, EventArgs e)
        {
            if (editarDAV == false)
            {
                this.editarDAV = true;
                bntEditarDAV.BackColor = Color.Red;
                this.mesclar = false;
                this.mesclarItens = false;
            }
            else
            {
                this.editarDAV = false;
                bntEditarDAV.BackColor = Color.LightSteelBlue;
                this.mesclar = false;
                this.mesclarItens = false;
            }

        }

        private void frmPreVenda_Shown(object sender, EventArgs e)
        {
            txtNumero.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnAppStore_Click(object sender, EventArgs e)
        {
            DivulgarPromocaoIQCARD.iniciarmostrandoPedido = true;
            DivulgarPromocaoIQCARD.iniciarmostrandoPromocao = false;
            ChamarAnuncio();
        }


        public void ChamarAnuncio()
        {
            if (!Funcoes.VerificarConexaoInternet())
            {
                MessageBox.Show("Sem internet");
                return;
            }
            DivulgarPromocaoIQCARD promo = new DivulgarPromocaoIQCARD();
            promo.ShowDialog();            
        }
    }    
}
