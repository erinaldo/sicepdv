using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Configuration;
using System.Data.EntityClient;
using System.Data;

namespace SICEpdv
{
    public static class FuncoesFiscais
    {
        public static bool camposAjustado { get; set; }



        public static string IndiceTotalizador(string aliquota,bool itemCancelado)
        {


            if (aliquota == "05")
                return "1";
            if (aliquota == "07")
                return "2";
            if (aliquota == "12")
                return "3";
            if (aliquota == "17")
                return "4";
            if (aliquota == "25")
                return "5";
            if (aliquota == "27")
                return "6";

            return "1";
        }

        public static string NumeroTotalizador(string aliquota, bool itemCancelado)
        {


            if (aliquota == "05")
                return "01T0500";
            if (aliquota == "07")
                return "02T0700";
            if (aliquota == "12")
                return "03T1200";
            if (aliquota == "17")
                return "04T1700";
            if (aliquota == "25")
                return "05T2500";
            if (aliquota == "27")
                return "06T2700";


            return "1";

        }       


    }

    public struct StruCodigos
    {
        public string codigo;
        public string filial;
    }

}
