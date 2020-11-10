using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICEpdv
{
    interface IcadastroProduto
    {
        string CadastrarProduto(produtos dados);
        string AlterarProduto(produtos dados);
        bool ValidarCampos(produtos dados);
        decimal FormarPreco(decimal custo, decimal margem);
        bool VerificarTributacao(produtos dados);
    }
}
