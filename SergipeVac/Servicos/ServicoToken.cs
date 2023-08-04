using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SergipeVac.Model.Autenticacao;
using SergipeVac.Model.Interface;

namespace SergipeVac.Servicos
{
    public class ServicoToken : IServicoToken
    {
        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var chaveSecreta = Configuracoes.ObterChave();

            var descricaoToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.Nome)
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(chaveSecreta, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateJwtSecurityToken(descricaoToken);

            return tokenHandler.WriteToken(token);
        }
    }
}
