using System.Text.Json.Serialization;

namespace SergipeVac.Model.DTOs.Autenticacao
{
    public class UsuarioAuthDTO
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("senha")]
        public string Senha { get; set; }
    }
}
