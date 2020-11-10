using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
   public static class GlbVariaveis
    {
       public static string glb_Usuario { get; set; }
       public static string glb_filial { get; set; } // Iniciada no Programm - Inicio de tudo!
       public static string glb_grupo { get; set; }
       public static string glb_IP { get; set; }
       public static bool glb_Acbr { get; set; }
       public static string glb_portaAcbr { get; set; }
       public static int glb_velocidadeAcbr { get; set; }
       public static bool glb_modoGavetaAcbr { get; set; }
       public static int glb_tempo { get; set; }
       public static bool glb_descricaoGrande { get; set; }
       public static bool glb_hardFlow { get; set; }
       public static bool glb_softFlow { get; set; }
       public static bool glb_clienteDAV { get; set; }
       public static string glb_estoque { get; set; }
       public static bool glb_balanca { get; set; }
       public static bool glb_standalone { get; set; }
       public static string glb_senhaUsuario { get; set; }
       public static string glb_gateway { get; set; }
       public static bool glb_TEFAcbr { get; set; }

        public static string glb_chaveIQCard { get; set; }

       // Identificação do cliente

       public static string idCliente { get; set; }
       public static string nomeEmpresa { get; set; }
       public static string telefone { get; set; }        
      

       // Informações de Homologação do PAF ECF
       public static string laudoPAF { get; set; }
       public static string versaoPAF { get; set; }
       public static int versaoSICENFCe { get; set; }
       public static int versaoSICEnet { get; set; }

        // Exclusivo IQ Sistemas
        public static string glbSenhaIQ { get; set; }
       public static string chavePrivada { get; set; }
       public static string iqcardsuporte { get; set; }
       public static string iqcardsuporteNome { get; set; }
       public static string whatsappsuporte { get; set; }

       public static string nomeAplicativo { get; set; }
       public static string nomeSH { get; set; }
       public static string razaoSH { get; set; }
       public static string cnpjSH { get; set; }
       public static string IESH { get; set; }
       public static string IEMunicipalSH { get; set; } 
       public static string enderecoSH { get; set; }
       public static string cepSH { get; set; }
       public static string bairroSH { get; set; }
       public static string cidadeSH { get; set; }
       public static string estadoSH { get; set; }
       public static string responsavelSH { get; set; }
       public static string contatoSH { get; set; }
       public static string telefoneSH { get; set; } 


       // Dados da Conexao com o DB
       //public static string glbIPServidor { get; set; }
       //public static string glbUser  { get; set; }
       //public static string glbPort { get; set; }
       //public static string glbDataBase { get; set; }
       //public static string glbPassword{ get; set; }
       public static string glb_Versao { get; set; }
       public static string glb_TipoPAF { get; set; }
       public static string glb_idCliente { get; set; }
       public static string glb_modoatualizacao { get; set; }
       public static string glb_horaAtualizacao { get; set; }
       public static string glb_atualizar { get; set; }
       public static string glb_rodarScript { get; set; }


        private static DateTime sys_Data;         

       public static DateTime Sys_Data
       {
           get { return GlbVariaveis.sys_Data = DateTime.Now.Date ; }
           set { GlbVariaveis.sys_Data = DateTime.Now.Date; }
       }

       public enum tipoPAF
       {
           Geral,
           Serviço,
           Combustível
       }

       /// 06 DAVEMITIDOS
       /// 07 IDPAFECF
       /// 08 MEIOSPGT
       /// 09 INDICEPROD
       /// 10 PARAMETROSCNF
       public enum TipoRelatorioGerencial
       {
           DAVEMITIDOS,
           DAVOSEMITIDOS,
           IDPAFECF,
           MEIOSPGT,
           INDICEPROD,
           PARAMETROSCNF,
           CONTACLIENTE
       }
    }
}
