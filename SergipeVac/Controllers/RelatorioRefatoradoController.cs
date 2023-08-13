using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SergipeVac.Model.Interface;
using SergipeVac.Model.ModeloDados;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioRefatoradoController : Controller
    {
        private readonly IRepositorio<Categoria> _repositorioCategoria;
        private readonly IRepositorio<DocumentoVacinacao> _repositorioDocumentoVacinacao;
        private readonly IRepositorio<Estabelecimento> _repositorioEstabelecimento;
        private readonly IRepositorio<Fabricante> _repositorioFabricantes;
        private readonly IRepositorio<GrupoAtendimento> _repositorioGrupoAtendimento;
        private readonly IRepositorio<Nacionalidade> _repositorioNacionalidade;
        private readonly IRepositorio<Paciente> _repositorioPaciente;
        private readonly IRepositorio<Raca> _repositorioRaca;
        private readonly IRepositorio<SexoBiologico> _repositorioSexoBiologico;

        public RelatorioRefatoradoController(IRepositorio<DocumentoVacinacao> repositorioDocumentoVacinacao,
                                             IRepositorio<Estabelecimento> repositorioEstabelecimento,
                                             IRepositorio<GrupoAtendimento> repositorioGrupoAtendimento,
                                             IRepositorio<Paciente> repositorioPaciente,
                                             IRepositorio<Raca> repositorioRaca,
                                             IRepositorio<SexoBiologico> repositorioSexoBiologico,
                                             IRepositorio<Nacionalidade> repositorioNacionalidade,
                                             IRepositorio<Fabricante> repositorioFabricantes,
                                             IRepositorio<Categoria> repositorioCategoria)
        {
            _repositorioDocumentoVacinacao = repositorioDocumentoVacinacao;
            _repositorioEstabelecimento = repositorioEstabelecimento;
            _repositorioGrupoAtendimento = repositorioGrupoAtendimento;
            _repositorioPaciente = repositorioPaciente;
            _repositorioRaca = repositorioRaca;
            _repositorioSexoBiologico = repositorioSexoBiologico;
            _repositorioNacionalidade = repositorioNacionalidade;
            _repositorioFabricantes = repositorioFabricantes;
            _repositorioCategoria = repositorioCategoria;
        }

        [HttpGet("contagemporetnia")]
        public async Task<IActionResult> ObterContagemPorEtnia(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var quantidadeDeVacinasAplicadasPorEtnia = _repositorioDocumentoVacinacao
                    .Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                    .Join(_repositorioPaciente.ObterTodos(), dv => dv.PacienteId, p => p.Id,
                        (dv, p) => new { Paciente = p, DocumentoVacinacao = dv })
                    .Join(_repositorioRaca.ObterTodos(), x => x.Paciente.RacaId, e => e.Id,
                        (x, e) => new { Etnia = e.Valor, x.DocumentoVacinacao })
                    .GroupBy(x => x.Etnia)
                    .Select(g => new
                    {
                        Etnia = g.Key,
                        QuantidadeVacinados = g.Count()
                    });

                return Json(quantidadeDeVacinasAplicadasPorEtnia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("contagemporestabelecimento")]
        public async Task<IActionResult> ObterTopEstabelecimentos(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var topEstabelecimentos = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                                                                        .GroupBy(x => x.EstabelecimentoId)
                                                                        .Select(g => new
                                                                        {
                                                                            EstabelecimentoId = g.Key,
                                                                            Frequencia = g.Count()
                                                                        })
                                                                        .OrderByDescending(x => x.Frequencia)
                                                                        .Take(quantidade);

                var topEstabelecimentosNomeados = topEstabelecimentos
                    .Join(
                        _repositorioEstabelecimento.Obter(est =>
                            topEstabelecimentos.Select(e => e.EstabelecimentoId).Contains(est.Id)
                        ),
                        dv => dv.EstabelecimentoId,
                        e => e.Id,
                        (dv, e) => new { DocumentoVacinacao = dv, Estabelecimento = e }
                    )
                    .GroupBy(x => x.Estabelecimento.NomeFantasia)
                    .Select(g => new
                    {
                        Estabelecimento = g.Key,
                        Frequencia = g.Sum(x => x.DocumentoVacinacao.Frequencia)
                    });

                return Json(topEstabelecimentosNomeados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("contagempornacionalidade")]
        public async Task<IActionResult> ObterNacionalidades(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var vacinasAplicadaEntreAsDatasInformadas = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc);

                var vacinadosPorNacionalidade = vacinasAplicadaEntreAsDatasInformadas
                    .Join(_repositorioPaciente.ObterTodos(),
                        dv => dv.PacienteId,
                        p => p.Id,
                        (dv, p) => new { Paciente = p, DocumentoVacinacao = dv })
                    .Join(_repositorioNacionalidade.ObterTodos(),
                        pac => pac.Paciente.NacionalidadeId,
                        na => na.Id,
                        (pac, na) => new { pac.Paciente, Nacionalidade = na })
                    .GroupBy(g => g.Nacionalidade)
                    .Select(g => new
                    {
                        Nacionalidade = g.Key,
                        TotalPacientes = g.Select(d => d.Paciente)
                            .Distinct()
                            .Count()
                    });


                return Json(vacinadosPorNacionalidade);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("contagemporsexo")]
        public async Task<IActionResult> ObterVacinadosPorSexoBiologico(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var vacinadosPorSexoBiologico = _repositorioDocumentoVacinacao
                    .Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                    .Join(_repositorioPaciente.ObterTodos(),
                        dv => dv.PacienteId,
                        p => p.Id,
                        (dv, p) => new { Paciente = p, DocumentoVacinacao = dv })
                    .Join(_repositorioSexoBiologico.ObterTodos(),
                        pac => pac.Paciente.SexoBiologicoId,
                        sb => sb.Id,
                        (pac, sb) => new { pac.Paciente, SexoBiologico = sb })
                    .GroupBy(g => g.SexoBiologico)
                    .Select(g => new
                    {
                        SexoBiologico = g.Key,
                        TotalPacientes = g.Select(d => d.Paciente.Id)
                            .Distinct()
                            .Count()
                    });

                return Json(vacinadosPorSexoBiologico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("contagempordose")]
        public async Task<IActionResult> ObterVacinadosPorDose(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var vacinasAplicadaEntreAsDatasInformadas = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc);

                var quantidadeDePacientesPorDoce = vacinasAplicadaEntreAsDatasInformadas
                                                                .GroupBy(d => d.DescricaoDose)
                                                                .Select(g => new
                                                                {
                                                                    VacinaDescricaoDose = g.Key,
                                                                    TotalPacientes = g.Select(d => d.PacienteId).Count()
                                                                })
                                                                .ToList();

                return Json(quantidadeDePacientesPorDoce);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("contagemporgrupo")]
        public async Task<IActionResult> ObterTopVacinasAplicadasPorGrupo(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var topGrupos = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                    .GroupBy(x => x.GrupoAtendimentoId)
                    .Select(g => new
                    {
                        GrupoAtendimentoId = g.Key,
                        Quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.Quantidade)
                    .Take(quantidade);

                var topGruposNomeados = topGrupos
                    .Join(
                        _repositorioGrupoAtendimento.Obter(est =>
                            topGrupos.Select(e => e.GrupoAtendimentoId).Contains(est.Id)
                        ),
                        dv => dv.GrupoAtendimentoId,
                        e => e.Id,
                        (dv, e) => new { DocumentoVacinacao = dv, GrupoAtendimento = e }
                    )
                    .GroupBy(x => x.GrupoAtendimento.Nome)
                    .Select(g => new
                    {
                        GrupoAtendimento = g.Key,
                        Frequencia = g.Sum(x => x.DocumentoVacinacao.Quantidade)
                    });

                return Json(topGruposNomeados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("contagemporcategoria")]
        public async Task<IActionResult> ObterTopVacinasAplicadasPorCategoria(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            try
            {
                dataMin ??= DateTime.MinValue;
                dataMax ??= DateTime.MaxValue;

                var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
                var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

                var topCategorias = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                    .GroupBy(x => x.CategoriaId)
                    .Select(g => new
                    {
                        CategoriaId = g.Key,
                        Quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.Quantidade)
                    .Take(quantidade);

                var topCategoriasNomeados = topCategorias
                    .Join(
                        _repositorioCategoria.Obter(cat =>
                            topCategorias.Select(e => e.CategoriaId).Contains(cat.Id)
                        ),
                        dv => dv.CategoriaId,
                        e => e.Id,
                        (dv, e) => new { DocumentoVacinacao = dv, Categoria = e }
                    )
                    .GroupBy(x => x.Categoria.Nome)
                    .Select(g => new
                    {
                        Categoria = g.Key,
                        Frequencia = g.Sum(x => x.DocumentoVacinacao.Quantidade)
                    });

                return Json(topCategoriasNomeados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("totalizadores")]
        public async Task<IActionResult> ObterTotalizadores()
        {
            try
            {
                var quantidadeDePacientesVacinados = _repositorioPaciente.QuantidadeTotal();
                var quantidadeDeVacinasAplicadas = _repositorioDocumentoVacinacao.QuantidadeTotal();
                var quantidadeDeEstrangeiros = _repositorioPaciente.QuantidadeDe(P => P.NacionalidadeId == 2);
                var numEstabelecimentos = _repositorioEstabelecimento.QuantidadeTotal();

                var topFabricante = _repositorioDocumentoVacinacao.ObterTodos()
                    .GroupBy(x => x.FabricanteId)
                    .Select(g => new
                    {
                        FabricanteId = g.Key,
                        Quantidade = g.Count()
                    })
                    .OrderByDescending(x => x.Quantidade)
                    .Take(1);

                var fabricanteComMaisDosesAplicadas = topFabricante
                    .Join(
                        _repositorioFabricantes.Obter(fab =>
                            topFabricante.Select(e => e.FabricanteId).Contains(fab.Id)
                        ),
                        dv => dv.FabricanteId,
                        e => e.Id,
                        (dv, e) => new { DocumentoVacinacao = dv, Fabricante = e }
                    ).Select(g => new
                    {
                        Fabricante = g.Fabricante.Nome,
                        g.DocumentoVacinacao.Quantidade
                    }).SingleOrDefault();

                var jsonData = new
                {
                    quantidadeDePacientesVacinados,
                    quantidadeDeVacinasAplicadas,
                    quantidadeDeEstrangeiros,
                    numEstabelecimentos,
                    fabricanteComMaisDosesAplicadas,
                };

                return Json(jsonData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
