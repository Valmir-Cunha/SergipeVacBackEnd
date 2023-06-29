using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model;
using SergipeVac.Model.Interface;

namespace SergipeVac.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : Controller
    {
        private readonly IRepositorio<DocumentoImportadoCSV> _repositorio;

        public RelatorioController(IRepositorio<DocumentoImportadoCSV> repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet("contagemporetnia")]
        public async Task<JsonResult> ObterContagemPorEtnia(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            // var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            // var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            // var documentos = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);


            // var vacinasPorEtnia = documentos
            //     .GroupBy(d => d.PacienteRacaCorValor)
            //     .Select(g => new
            //     {
            //         PacienteRacaCorValor = g.Key,
            //         TotalPacientes = g.Select(d => d.PacienteId).Distinct().Count()
            //     })
            //     .ToList();

            // return Json(vacinasPorEtnia);
            
              var data = new[]
            {
            new { pacienteRacaCorValor = "", totalPacientes = 1 },
            new { pacienteRacaCorValor = "AMARELA", totalPacientes = 113604 },
            new { pacienteRacaCorValor = "BRANCA", totalPacientes = 61903 },
            new { pacienteRacaCorValor = "INDIGENA", totalPacientes = 559 },
            new { pacienteRacaCorValor = "PARDA", totalPacientes = 140220 },
            new { pacienteRacaCorValor = "PRETA", totalPacientes = 16505 },
            new { pacienteRacaCorValor = "SEM INFORMACAO", totalPacientes = 339506 }
        };

            return Json(data);
        }

        [HttpGet("contagemporano")]
        public async Task<JsonResult> ObterContagemVacinasPorAno(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc).ToList();

            var vacinasAgrupadasPorAno = vacinasAplicadaEntreAsDatasInformadas
                .GroupBy(v => v.VacinaDataAplicacao.Year)
                .Select(g => new
                {
                    Ano = g.Key,
                    TotalVacinas = g.Count()
                })
                .ToList();

            return Json(vacinasAgrupadasPorAno);
        }

        [HttpGet("contagemporestabelecimento")]
        public async Task<JsonResult> ObterTopEstabelecimentos(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var topEstabelecimentos = vacinasAplicadaEntreAsDatasInformadas.GroupBy(d => d.EstabelecimentoNoFantasia)
                                                                           .OrderByDescending(g => g.Count())
                                                                           .Take(quantidade)
                                                                           .Select(g => new
                                                                           {
                                                                               Estabelecimento = g.Key,
                                                                               Frequencia = g.Count()
                                                                           })
                                                                           .ToList();

            return Json(topEstabelecimentos);
        }

        [HttpGet("contagempornacionalidade")]
        public async Task<JsonResult> ObterNacionalidades(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var vacinadosPorNacionalidade = vacinasAplicadaEntreAsDatasInformadas
                                    .GroupBy(d => d.PacienteNacionalidadeEnumNacionalidade == "" ||
                                                  d.PacienteNacionalidadeEnumNacionalidade == "None" ||
                                                  d.PacienteNacionalidadeEnumNacionalidade == "N" ? "Sem informação" :
                                                  d.PacienteNacionalidadeEnumNacionalidade == "B" ? "Brasileiro" :
                                                  d.PacienteNacionalidadeEnumNacionalidade == "E" ? "Estrangeiro" :
                                                  d.PacienteNacionalidadeEnumNacionalidade)
                                    .Select(g => new
                                    {
                                        Nacionalidade = g.Key,
                                        TotalPacientes = g.Select(d => d.PacienteId)
                                                          .Distinct()
                                                          .Count()
                                    })
                                    .OrderByDescending(g => g.TotalPacientes)
                                    .ToList();

            return Json(vacinadosPorNacionalidade);
        }

        [HttpGet("contagemporsexo")]
        public async Task<JsonResult> ObterVacinadosPorSexoBiologico(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var vacinadosPorSexoBiologico = vacinasAplicadaEntreAsDatasInformadas
                                                                .GroupBy(d =>
                                                                    d.PacienteEnumSexoBiologico == "F" ? "Feminino" :
                                                                    d.PacienteEnumSexoBiologico == "M" ? "Masculino" :
                                                                    d.PacienteEnumSexoBiologico == "I" ? "Intersexo" :
                                                                    "Sem informação")
                                                                .Select(g => new
                                                                {
                                                                    SexoBiologico = g.Key,
                                                                    TotalPacientes = g.Select(d => d.PacienteId)
                                                                                      .Distinct()
                                                                                      .Count()
                                                                })
                                                                .ToList();

            return Json(vacinadosPorSexoBiologico);
        }

        [HttpGet("contagempordose")]
        public async Task<JsonResult> ObterVacinadosPorDose(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var quantidadeDePacientesPorDoce = vacinasAplicadaEntreAsDatasInformadas
                                                            .GroupBy(d => d.VacinaDescricaoDose)
                                                            .Select(g => new
                                                            {
                                                                VacinaDescricaoDose = g.Key,
                                                                TotalPacientes = g.Select(d => d.PacienteId).Count()
                                                            })
                                                            .ToList();

            return Json(quantidadeDePacientesPorDoce);
        }

        [HttpGet("contagemporidade")]
        public async Task<JsonResult> ObterVacinasAplicadasPorIdade(int idadeInicial = 0, int idadeFinal = 120,DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc && p.PacienteIdade >= idadeInicial && p.PacienteIdade <= idadeFinal);

            var quantidadeDeVacinasPorIdade= vacinasAplicadaEntreAsDatasInformadas.GroupBy(d => d.PacienteIdade)
                                                                .Select(g => new
                                                                {
                                                                    PacienteIdade = g.Key,
                                                                    TotalPacientes = g.Select(d => d.PacienteId)
                                                                                      .Distinct()
                                                                                      .Count()
                                                                })
                                                                .ToList();

            return Json(quantidadeDeVacinasPorIdade);
        }

        [HttpGet("contagemporsistema")]
        public async Task<JsonResult> ObterVacinasRegistradasPorSistema(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var quantidadeDeVacinasRegistradasPorSistema = vacinasAplicadaEntreAsDatasInformadas.GroupBy(d => d.SistemaOrigem)
                .Select(g => new
                {
                    SistemaOrigem = g.Key,
                    TotalPacientes = g.Select(d => d.PacienteId).Count()
                })
                .ToList();

            return Json(quantidadeDeVacinasRegistradasPorSistema);
        }

        [HttpGet("contagemporvacinas")]
        public async Task<JsonResult> ObterTopVacinasAplicadas(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var topVacinas = vacinasAplicadaEntreAsDatasInformadas.GroupBy(d => d.VacinaNome)
                .OrderByDescending(g => g.Count())
                .Take(quantidade)
                .Select(g => new
                {
                    VacinaNome = g.Key,
                    Frequencia = g.Count()
                })
                .ToList();

            return Json(topVacinas);
        }

        [HttpGet("contagemporgrupo")]
        public async Task<JsonResult> ObterTopVacinasAplicadasPorGrupo(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAplicadaEntreAsDatasInformadas = _repositorio.Obter(p => p.VacinaDataAplicacao >= dataMinUtc && p.VacinaDataAplicacao <= dataMaxUtc);

            var topGrupos = vacinasAplicadaEntreAsDatasInformadas.GroupBy(d => d.VacinaGrupoAtendimentoNome)
                .OrderByDescending(g => g.Count())
                .Take(quantidade)
                .Select(g => new
                {
                    VacinaGrupoAtendimentoNome = g.Key,
                    Frequencia = g.Count()
                })
                .ToList();

            return Json(topGrupos);
        }
    }
}
