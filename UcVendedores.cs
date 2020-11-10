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
    public partial class UcVendedores : UserControl
    {
        public delegate void ClicarNoBotao(object sender);
        public event ClicarNoBotao clickBotao;
        public UcVendedores()
        {
            InitializeComponent();
        }

        private void UcVendedores_Load(object sender, EventArgs e)
        {
            int x = 0;
            int y = 0;

            siceEntities entidade;
            if (Conexao.tipoConexao == 2 && !Conexao.ConexaoOnline())
                entidade = Conexao.CriarEntidade(false);
            else
                entidade = Conexao.CriarEntidade();


            var vendedores = from n in entidade.vendedores
                             where n.CodigoFilial == GlbVariaveis.glb_filial
                             && n.ativo == "S"
                             orderby n.codigo
                             select new { n.codigo, n.nome };
            foreach (var item in vendedores)
            {
                if (x > 6)
                {
                    x = 0;
                    y = y + 44; ;
                }

                Button btn = new Button();
                btn.BackColor = Color.FromArgb(91, 191, 223);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Margin = new Padding(20);

                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 65, 119);
                btn.FlatAppearance.BorderSize = 3;

                // btn.FlatAppearance = BorderStyle.None;
                btn.Name = item.codigo;
                btn.Text = item.codigo + " - " + item.nome;
                btn.Size = new Size(98, 50);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Location = new System.Drawing.Point(x * 93, y);
                btn.Click += (objeto, evento) =>
                {
                    clickBotao(objeto);
                    Vendedor.VendaVendedor(btn.Name);
                };
                pnlVendedores.Controls.Add(btn);
                x++;
            }
        }

       
    }
}
