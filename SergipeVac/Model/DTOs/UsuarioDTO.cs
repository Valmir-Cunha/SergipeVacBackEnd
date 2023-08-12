using System.Text.Json.Serialization;

namespace SergipeVac.Model.DTOs
{
    public class UsuarioDTO
    {
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }
        
        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

    }
}
