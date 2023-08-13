using RestSharp;

namespace SergipeVac.Servicos
{
    public class ServicoSincronizacao
    {
        private readonly HttpClient _httpClient;

        public ServicoSincronizacao(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SincronizarDadosAsync()
        {
            await ObterDadosASincronizarAsync();
        }

        private async Task ObterDadosASincronizarAsync()
        {
            try
            {
                var opcoes = new RestClientOptions("https://imunizacao-es.saude.gov.br")
                {
                    MaxTimeout = -1,
                };
                var cliente = new RestClient(opcoes);
                var requisicao = new RestRequest("/_search");
                requisicao.AddHeader("Authorization", "Basic aW11bml6YWNhb19wdWJsaWM6cWx0bzV0JjdyX0ArI1Rsc3RpZ2k=");
                RestResponse respostaRequisicao = await cliente.ExecuteAsync(requisicao);

                if (respostaRequisicao.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var conteudoResposta = respostaRequisicao.Content;
                    var dadosVacinas = Newtonsoft.Json.JsonConvert.DeserializeObject(conteudoResposta);
                    Console.WriteLine(dadosVacinas);
                }
                else
                {
                    Console.WriteLine("Erro na requisição: " + respostaRequisicao.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }
    }
}