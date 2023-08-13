using Newtonsoft.Json.Linq;
using RestSharp;
using SergipeVac.Model.ModeloDados;
using System.ComponentModel.DataAnnotations.Schema;

namespace SergipeVac.Servicos
{
    public class ServicoSincronizacao
    {
        public async Task SincronizarDadosAsync()
        {
            await ObterDadosASincronizarAsync();
        }

        private async Task ObterDadosASincronizarAsync()
        {
            try
            {
                var listaDocumentosImportados = new List<DocumentoImportado>();

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
                    var jsonResposta = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(conteudoResposta);

                    var dadosVacinacaoJSON = jsonResposta["hits"]["hits"];

                    var totalDeRegistros = jsonResposta["hits"]["total"]["value"];

                    if (totalDeRegistros != null && (int) totalDeRegistros > 10000)
                    {
                        Console.WriteLine(totalDeRegistros);
                    }

                    foreach (var hit in dadosVacinacaoJSON)
                    {
                        var documentoVacinacaoJSON = hit["_source"];

                        var documentoVacinacaoImportado = ObterDocumentoImportado(documentoVacinacaoJSON);

                        listaDocumentosImportados.Add(documentoVacinacaoImportado);
                    }

                    Console.WriteLine(listaDocumentosImportados.Count);
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
    }
}