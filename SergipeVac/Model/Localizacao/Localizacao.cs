using System.Text.Json.Serialization;

namespace SergipeVac.Model.Localizacao
{
    public class Localizacao
    {
        public int Id { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("cidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("quantidade")]
        public int Quantidade { get; set; }
    }
}
