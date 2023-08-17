using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SergipeVac.Conversores;
using SergipeVac.Infra.Repositorios;
using SergipeVac.Model.ModeloDados;
using SergipeVac.Model.Sincronizacao;

namespace SergipeVac.Servicos
{
    public class ServicoSincronizacao
    {
        private readonly int _totalDeResgistrosPorRequisicao = 10000;
        private List<DocumentoImportado> ListaDocumentosImportados { get; set; }
        private RepositorioSincronizacao RepositorioSincronizacao { get; set; }
        private ConversorDados ConversorDados { get; set; }

        private DateTime _inicioSincronizacao;


        public ServicoSincronizacao(RepositorioSincronizacao repositorioSincronizacao, ConversorDados conversorDados)
        {
            RepositorioSincronizacao = repositorioSincronizacao;
            ConversorDados = conversorDados;
            ListaDocumentosImportados = new List<DocumentoImportado>();
            _inicioSincronizacao = DateTime.Now;
        }

        public async Task SincronizarDadosAsync()
        {
            await ObterDadosASincronizarAsync();
        }

        private async Task ObterDadosASincronizarAsync()
        {
            try
            {
                var bodyRequisicao = ObterDadosAPartirDaUltimaSincronizacao();

                RestResponse respostaRequisicao = await RealizarRequisicaoUltimosRegistrosAsync(bodyRequisicao, "/_search?scroll=1m");

                if (respostaRequisicao.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var conteudoResposta = respostaRequisicao.Content;
                    var jsonResposta = JsonConvert.DeserializeObject<JObject>(conteudoResposta);

                    var dadosVacinacaoJSON = jsonResposta["hits"]["hits"];

                    var totalDeRegistros = jsonResposta["hits"]["total"]["value"];

                    if (totalDeRegistros != null && (int) totalDeRegistros > _totalDeResgistrosPorRequisicao)
                    {
                        var scroll = jsonResposta["_scroll_id"];
                        await ObterRegistrosComScroll(scroll, dadosVacinacaoJSON, totalDeRegistros);
                        Console.WriteLine(totalDeRegistros);
                    }
                    else
                    {
                        ObterDocumentosImportadosJSON(dadosVacinacaoJSON);
                    }

                    ConverterDados();
                    SalvarSincronizacaoBemSucedida();
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

        private async Task<RestResponse> RealizarRequisicaoUltimosRegistrosAsync(string bodyRequisicao, string endPoint)
        {
            try
            {
                var opcoes = new RestClientOptions("https://imunizacao-es.saude.gov.br")
                {
                    MaxTimeout = -1,
                };
                var cliente = new RestClient(opcoes);
                var requisicao = new RestRequest(endPoint);

                requisicao.AddHeader("Content-Type", "application/json");
                requisicao.AddHeader("Authorization", "Basic aW11bml6YWNhb19wdWJsaWM6cWx0bzV0JjdyX0ArI1Rsc3RpZ2k=");

                requisicao.AddStringBody(bodyRequisicao, DataFormat.Json);

                return await cliente.ExecuteAsync(requisicao);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro na requisição: " + ex.Message);
                return null;
            }
        }

        private async Task ObterRegistrosComScroll(JToken scrollPrimeiraReq, JToken dadosVacinacaoJSON, JToken totalRegistros)
        {
            var quantidadeDeScrool = Math.Ceiling((decimal) totalRegistros / _totalDeResgistrosPorRequisicao);
            var scroll = scrollPrimeiraReq;
            var dadosVacinacoes = dadosVacinacaoJSON;
            for (var rodada = 1; rodada <= quantidadeDeScrool; rodada++)
            {
                ObterDocumentosImportadosJSON(dadosVacinacoes);
                RestResponse respostaRequisicao = await ObterProximoScrollAPI(scroll);
                if (respostaRequisicao.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResposta = JsonConvert.DeserializeObject<JObject>(respostaRequisicao.Content);

                    dadosVacinacoes = jsonResposta["hits"]["hits"];
                }
                else
                {
                    break;
                }
            }
        }

        private void ObterDocumentosImportadosJSON(JToken dadosVacinacaoJSON)
        {
            foreach (var hit in dadosVacinacaoJSON)
            {
                var documentoVacinacaoJSON = hit["_source"];

                var documentoVacinacaoImportado = ObterDocumentoImportado(documentoVacinacaoJSON);

                ListaDocumentosImportados.Add(documentoVacinacaoImportado);
            }
        }

        public async Task<RestResponse> ObterProximoScrollAPI(JToken scroll)
        {
            var bodyRequisicao = ObterDadosDoProximoScroll((string) scroll);
            RestResponse respostaRequisicao = await RealizarRequisicaoUltimosRegistrosAsync(bodyRequisicao, "/_search/scroll");
            return respostaRequisicao;
        }

        private DocumentoImportado ObterDocumentoImportado(JToken documentoVacinacaoJson)
        {
            if (documentoVacinacaoJson != null)
            {
                var documentoImportado = new DocumentoImportado
                {
                    DocumentId = (string) documentoVacinacaoJson["document_id"],

                    // Paciente
                    PacienteId = (string) documentoVacinacaoJson["paciente_id"],
                    PacienteIdade = (int) documentoVacinacaoJson["paciente_idade"],
                    PacienteDataNascimento = (DateTime) documentoVacinacaoJson["paciente_dataNascimento"],
                    PacienteEnumSexoBiologico = (string) documentoVacinacaoJson["paciente_enumSexoBiologico"],
                    PacienteRacaCorCodigo = (int) documentoVacinacaoJson["paciente_racaCor_codigo"],
                    PacienteRacaCorValor = (string) documentoVacinacaoJson["paciente_racaCor_valor"],
                    PacienteEnderecoCoibgeMunicipio = (int) documentoVacinacaoJson["paciente_endereco_coIbgeMunicipio"],
                    PacienteEnderecoCoPais = (int) documentoVacinacaoJson["paciente_endereco_coPais"],
                    PacienteEnderecoNmMunicipio = (string) documentoVacinacaoJson["paciente_endereco_nmMunicipio"],
                    PacienteEnderecoNmPais = (string) documentoVacinacaoJson["paciente_endereco_nmPais"],
                    PacienteEnderecoUF = (string) documentoVacinacaoJson["paciente_endereco_uf"],
                    PacienteEnderecoCEP = (string) documentoVacinacaoJson["paciente_endereco_cep"],
                    PacienteNacionalidadeEnumNacionalidade = (string) documentoVacinacaoJson["paciente_nacionalidade_enumNacionalidade"],

                    // Estabelecimento
                    EstabelecimentoValor = (int) 0, // Não está na API
                    EstabelecimentoRazaoSocial = (string) documentoVacinacaoJson["estabelecimento_razaoSocial"],
                    EstabelecimentoNoFantasia = (string) documentoVacinacaoJson["estalecimento_noFantasia"],
                    EstabelecimentoMunicipioCodigo = (int) documentoVacinacaoJson["estabelecimento_municipio_codigo"],
                    EstabelecimentoMunicipioNome = (string) documentoVacinacaoJson["estabelecimento_municipio_nome"],
                    EstabelecimentoUF = (string) documentoVacinacaoJson["estabelecimento_uf"],

                    // Vacina
                    VacinaGrupoAtendimentoCodigo = (int) documentoVacinacaoJson["vacina_grupoAtendimento_codigo"],
                    VacinaGrupoAtendimentoNome = (string) documentoVacinacaoJson["vacina_grupoAtendimento_nome"],
                    VacinaCategoriaCodigo = (int) documentoVacinacaoJson["vacina_categoria_codigo"],
                    VacinaCategoriaNome = (string) documentoVacinacaoJson["vacina_categoria_nome"],
                    VacinaLote = (string) documentoVacinacaoJson["vacina_lote"],
                    VacinaFabricanteNome = (string) documentoVacinacaoJson["vacina_fabricante_nome"],
                    VacinaFabricanteReferencia = (string) documentoVacinacaoJson["vacina_fabricante_referencia"],
                    VacinaDataAplicacao = (DateTime) documentoVacinacaoJson["vacina_dataAplicacao"],
                    VacinaDescricaoDose = (string) documentoVacinacaoJson["vacina_descricao_dose"],
                    VacinaCodigo = (int) documentoVacinacaoJson["vacina_codigo"],
                    VacinaNome = (string) documentoVacinacaoJson["vacina_nome"],

                    // Sistema Origem
                    SistemaOrigem = (string) documentoVacinacaoJson["sistema_origem"]
                };

                return documentoImportado;
            }

            return null;
        }

        public string ObterDadosAPartirDaUltimaSincronizacao()
        {
            DateTime dataUltimaSincronizacao = RepositorioSincronizacao.ObterUltimaSincronizacaoBemSucedida();
            if (dataUltimaSincronizacao == null || dataUltimaSincronizacao == DateTime.MinValue.Date)
            {
                Console.WriteLine("Erro ao obter última atualização! \nSincronização não será realizada!");
                throw new Exception("Erro ao obter última atualização!");
            }
            int codigoAracajuIBGE = 280030;
            var ufSergipe = "SE";

            var bodyRequisicao =
            $@"{{
                ""size"": ""{_totalDeResgistrosPorRequisicao}"",
                ""track_total_hits"": true,
                ""query"": {{
                    ""bool"": {{
                        ""filter"": [
                            {{
                                ""range"": {{
                                    ""vacina_dataAplicacao"": {{
                                        ""gte"": ""{dataUltimaSincronizacao:yyyy-MM-dd}""
                                    }}
                                }}
                            }},
                            {{
                                ""match"": {{
                                    ""estabelecimento_municipio_codigo"": ""{codigoAracajuIBGE}""
                                }}
                            }},
                            {{
                                ""match"": {{
                                    ""paciente_endereco_coIbgeMunicipio"": ""{codigoAracajuIBGE}""
                                }}
                            }},
                            {{
                                ""match"": {{
                                    ""estabelecimento_uf"": ""{ufSergipe}""
                                }}
                            }},
                            {{
                                ""match"": {{
                                    ""status"": ""final""
                                }}
                            }}
                        ]
                    }}
                }}
            }}";

            return bodyRequisicao;
        }

        public string ObterDadosDoProximoScroll(string scroll)
        {
            var bodyRequisicao =
                $@"{{
                ""scroll_id"": ""{scroll}"",
                ""scroll"": ""2m""
                }}";

            return bodyRequisicao;
        }

        private void ConverterDados()
        {
            ConversorDados.ConverterDocumentoImportadoParaDocumentosVacinacao(ListaDocumentosImportados);
        }
        private void SalvarSincronizacaoBemSucedida()
        {
            DadosSincronizacao dadosSincronizacao = new DadosSincronizacao
            {
                BemSucedida = true,
                QuantidadeRegistrosAdicionados = ListaDocumentosImportados.Count,
                UltimaSincronizacao = _inicioSincronizacao,
            };
            RepositorioSincronizacao.Adicionar(dadosSincronizacao);
        }
    }
}