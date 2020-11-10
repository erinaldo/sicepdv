using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    class LogSICEpdv
    {
        static public void Registrarlog(string comando, string resultado, string classe)
        {
            return;
            criarArquivoLog();

            string conteudoTxt, novoConteudoTxt;

            string caminhoAbrir = @"C:\iqsistemas\LogSICEpdv.txt";
            //Jogar o conteúdo do arquivo numa string
            conteudoTxt = File.ReadAllText(caminhoAbrir);

            //Faz as substituições
            novoConteudoTxt = conteudoTxt = conteudoTxt +  "\n\r---------------------------------------------------------------------------------------------------------------------------------------------\n\r" +
                                                            "\n\r" + "Req.:" + comando + "->Resp.:" + resultado + "->Arq.:" + classe + "->Dt" + DateTime.Now.Date.ToShortDateString() + "->hr" + DateTime.Now.TimeOfDay.ToString() + "\n\r" +
                                                            "\n\r-----------------------------------------------------------------------------------------------------------------------------------------------\n\r";
   
            //Pegar o caminho do arquivo que ele criou
            string caminhoSalvar = caminhoAbrir;
            //Salvar todo o texto no caminho do arquivo escolhido
            File.WriteAllText(caminhoSalvar, novoConteudoTxt);
                

        }

        static private void criarArquivoLog()
        {
            return;
            if (!File.Exists(@"c:\iqsistemas\LogSICEpdv.txt"))
            {
                File.Create(@"c:\iqsistemas\LogSICEpdv.txt").Close();
            }
        }
    }
}
