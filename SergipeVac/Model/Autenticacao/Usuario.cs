using System.Text.Json.Serialization;

namespace SergipeVac.Model.Autenticacao
{
    public class Usuario
    {
        public int Codigo{ get; set; }
        [JsonPropertyName("nome")]
        public string Nome { get; set; }
        [JsonPropertyName("email")]
        public string Email{ get; set; }
        [JsonPropertyName("senha")]
        public string Senha{ get; set; }
    }
}
