using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SergipeVac
{
    public static class Configuracoes
    {
        public static SymmetricSecurityKey ObterChave()
        {
            string chaveSecreta = "chaveSecretaUtilizadaParaOSistemaDeAutenticacaoNaDisciplinaDeProgramacaoWeb2023";

            byte[] bytesChave = Encoding.UTF8.GetBytes(chaveSecreta);

            var chaveSecretaCodificada = new SymmetricSecurityKey(bytesChave);

            return chaveSecretaCodificada;
        }
    }
}
