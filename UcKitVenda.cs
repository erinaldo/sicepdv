using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SICEpdv
{
    public partial class UcKitVenda : UserControl
    {
        
        public UcKitVenda()
        {
            InitializeComponent();
        }

        public delegate void ClicarNoCurrentBotao(object sender);
        public event ClicarNoCurrentBotao clickBotao;

        private void UcKitVenda_Load(object sender, EventArgs e)
        {
           int x = 0;
            int y = 0;

            siceEntities entidade = Conexao.CriarEntidade();
            if (Conexao.tipoConexao == 2)
                entidade = Conexao.CriarEntidade(false);

            var Kits = from n in entidade.contvendapadronizada
                             where n.codigofilial == GlbVariaveis.glb_filial
                             orderby n.descricao
                             select new { n.numero, n.descricao};
            foreach (var Kit in Kits)
            {
                if (x > 3)
                {
                    x = 0;
                    y = y + 44; ;
                }

                Button btn = new Button();
                btn.Name = Kit.numero.ToString();
                btn.Text = Kit.numero + " - " + Kit.descricao;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.BackColor = Color.FromArgb(91, 191, 223);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Size = new Size(110, 44);
                btn.Location = new System.Drawing.Point(x * 103, y);
                btn.Click += (objeto, evento) =>
                {
                    clickBotao(objeto);
                    //Vendedor.VendaVendedor(btn.Name);
                   FrmKitVenda.kitNumero = Convert.ToInt16(btn.Name);
                };
                pnlKitVenda.Controls.Add(btn);
                x++;
            }
            this.pnlKitVenda.Size = new Size(480, y + 90);
            if (y > 630) y = 630;
            this.Size = new Size(480, y + 90);

            // this.BackColor = Color.Azure;
        }
    }
}
