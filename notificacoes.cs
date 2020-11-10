using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    class notificacoes
    {
        public string notificacao { get; set; }
        public string codigoFilial { get; set; }

        public static void addNotificacao(notificacoes notificao)
        {
            Conexao.CriarEntidade().ExecuteStoreCommand("insert into notificacoes(notificacao,visualizada,operador,DATA,dataVisualizacao,horas,horasVisualizacao,codigoFilial) values " +
                                                                                "'"+notificao.notificacao+"','N','',current_date,NULL,current_time,NULL,'"+notificao.codigoFilial+"'");
        }

    }
}
