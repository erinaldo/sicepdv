using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SICEpdv
{
    class Toledo
    {
        [DllImport("P05.dll")]
        public static extern int AbrePorta(int porta, int velocidade, int dataBits, int paridade);

        [DllImport("P05.dll")]
        public static extern int FechaPorta();

        [DllImport("P05.dll")]
        public static extern int PegaPeso(int tipoEscrita, StringBuilder peso, string diretorio);
    }
}
