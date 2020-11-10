using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace SICEpdv
{
    class webServices
    {
        public void versao()
        {
            try
            {
                var entidade = Conexao.CriarEntidade();

                var dados = (from i in entidade.iqsistemas select i).FirstOrDefault();
                string versaobanco = entidade.ExecuteFunction<string>("SELECT versaodb FROM iqsistemas LIMIT 1").FirstOrDefault();

                using (var client = new WebClient())
                {
                    var values = new NameValueCollection();
                    values["idCliente"] = dados.idcliente.ToString();
                    values["fantasia"] = GlbVariaveis.glb_filial;
                    values["versaosicenet"] = "";
                    values["versaosicenfce"] = "";
                    values["versaosicenfe"] = "";
                    values["versaosicepdv"] = GlbVariaveis.glb_Versao;
                    values["versaosicemdf"] = "";
                    values["versaodb"] = versaobanco;
                    values["terminal"] = GlbVariaveis.glb_IP;
                    values["tokem"] = "940eea71ef08de7bd692048dc6c01dde";

                    var response = client.UploadValues("http://localhost:8081/atualizacao/status.php", values);

                    var responseString = Encoding.Default.GetString(response);

                }
            }
            catch (Exception erro)
            {
                //MessageBox.Show(erro.ToString());
            }
        }
    }
}
