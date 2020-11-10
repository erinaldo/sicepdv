using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SICEpdv
{
    class Cedulas
    {
         public int id { get; set; }
        public decimal valor { get; set; }
        public Image img  { get; set; }


        public List<Cedulas> ListagemCedulas()
        {
            var dados = new[]
            {                
                new Cedulas{id=2,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\2-real.png"),valor=2M},
                new Cedulas{id=3,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\5-real.png"),valor=5M},
                new Cedulas{id=4,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\10-real.png"),valor=10M},
                new Cedulas{id=5,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\20-real.png"),valor=20M},
                new Cedulas{id=6,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\50-real.png"),valor=50M},
                new Cedulas{id=7,img=Image.FromFile(@Application.StartupPath+@"\imagens\moeda\100-real.png"),valor=100M}               
            };

            return dados.ToList();
        }         
    }
}
