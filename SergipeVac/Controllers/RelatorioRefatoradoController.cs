﻿using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SergipeVac.Model;
using SergipeVac.Model.Interface;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioRefatoradoController : Controller
    {
        private readonly IRepositorio<DocumentoImportadoCSV> _repositorio;
        private readonly IRepositorio<DocumentoVacinacao> _repositorioDocumentoVacinacao;
        private readonly IRepositorio<Estabelecimento> _repositorioEstabelecimento;
        private readonly IRepositorio<GrupoAtendimento> _repositorioGrupoAtendimento;
        private readonly IRepositorio<Nacionalidade> _repositorioNacionalidade;
        private readonly IRepositorio<Paciente> _repositorioPaciente;
        private readonly IRepositorio<Raca> _repositorioRaca;
        private readonly IRepositorio<SexoBiologico> _repositorioSexoBiologico;
        private readonly IRepositorio<Sistema> _repositorioSistema;

        public RelatorioRefatoradoController(IRepositorio<DocumentoImportadoCSV> repositorio,
                                             IRepositorio<DocumentoVacinacao> repositorioDocumentoVacinacao,
                                             IRepositorio<Estabelecimento> repositorioEstabelecimento,
                                             IRepositorio<GrupoAtendimento> repositorioGrupoAtendimento,
                                             IRepositorio<Paciente> repositorioPaciente,
                                             IRepositorio<Raca> repositorioRaca,
                                             IRepositorio<SexoBiologico> repositorioSexoBiologico,
                                             IRepositorio<Sistema> repositorioSistema, 
                                             IRepositorio<Nacionalidade> repositorioNacionalidade)
        {
            _repositorio = repositorio;
            _repositorioDocumentoVacinacao = repositorioDocumentoVacinacao;
            _repositorioEstabelecimento = repositorioEstabelecimento;
            _repositorioGrupoAtendimento = repositorioGrupoAtendimento;
            _repositorioPaciente = repositorioPaciente;
            _repositorioRaca = repositorioRaca;
            _repositorioSexoBiologico = repositorioSexoBiologico;
            _repositorioSistema = repositorioSistema;
            _repositorioNacionalidade = repositorioNacionalidade;
        }

        [HttpGet("contagemporetnia")]
        public async Task<JsonResult> ObterContagemPorEtnia(DateTime? dataMin = null, DateTime? dataMax = null)
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

        [HttpGet("contagemporano")]
        public async Task<JsonResult> ObterContagemVacinasPorAno(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var vacinasAgrupadasPorAno = _repositorioDocumentoVacinacao
                .Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                .GroupBy(v => new { v.DataAplicacao.Year })
                .Select(g => new
                {
                    Ano = g.Key.Year,
                    TotalVacinas = g.Count()
                });

            return Json(vacinasAgrupadasPorAno);
        }


        [HttpGet("contagemporestabelecimento")]
        public async Task<JsonResult> ObterTopEstabelecimentos(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
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

        [HttpGet("contagempornacionalidade")]
        public async Task<JsonResult> ObterNacionalidades(DateTime? dataMin = null, DateTime? dataMax = null)
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

        [HttpGet("contagemporsexo")]
        public async Task<JsonResult> ObterVacinadosPorSexoBiologico(DateTime? dataMin = null, DateTime? dataMax = null)
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
        public async Task<JsonResult> ObterVacinasAplicadasPorIdade(int idadeInicial = 0, int idadeFinal = 120, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var quantidadeDeVacinasAplicadasPorIdade = _repositorioDocumentoVacinacao
                .Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                .Join(_repositorioPaciente.Obter(p => p.Idade>= idadeInicial && p.Idade <= idadeFinal), dv => dv.PacienteId, p => p.Id,
                    (dv, p) => new { Paciente = p, DocumentoVacinacao = dv })
                .GroupBy(x => x.Paciente.Idade)
                .Select(g => new
                {
                    Idade = g.Key,
                    QuantidadeVacinados = g.Count()
                });

            return Json(quantidadeDeVacinasAplicadasPorIdade);
        }

        [HttpGet("contagemporsistema")]
        public async Task<JsonResult> ObterVacinasRegistradasPorSistema(DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var quantidadeDeVacinasRegistradasPorSistema = _repositorioDocumentoVacinacao
                .Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                .Join(_repositorioSistema.ObterTodos(), dv => dv.SistemaOrigemId, p => p.Id,
                    (dv, p) => new { Sistema = p, DocumentoVacinacao = dv })
                .GroupBy(x => x.Sistema.Nome)
                .Select(g => new
                {
                    Sistema = g.Key,
                    QuantidadeVacinadosSistema = g.Count()
                });

            return Json(quantidadeDeVacinasRegistradasPorSistema);
        }

        [HttpGet("contagemporvacinas")]
        public async Task<JsonResult> ObterTopVacinasAplicadas(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
        {
            dataMin ??= DateTime.MinValue;
            dataMax ??= DateTime.MaxValue;

            var dataMinUtc = new DateTimeOffset(dataMin.Value, TimeSpan.Zero);
            var dataMaxUtc = new DateTimeOffset(dataMax.Value, TimeSpan.Zero);

            var topVacinas = _repositorioDocumentoVacinacao.Obter(p => p.DataAplicacao >= dataMinUtc && p.DataAplicacao <= dataMaxUtc)
                .GroupBy(x => x.Nome)
                .Select(g => new
                {
                    Vacina = g.Key,
                    Quantidade = g.Count()
                })
                .OrderByDescending(x => x.Quantidade)
                .Take(quantidade);

            return Json(topVacinas);
        }

        [HttpGet("contagemporgrupo")]
        public async Task<JsonResult> ObterTopVacinasAplicadasPorGrupo(int quantidade = 10, DateTime? dataMin = null, DateTime? dataMax = null)
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
                    Estabelecimento = g.Key,
                    Frequencia = g.Sum(x => x.DocumentoVacinacao.Quantidade)
                });

            return Json(topGruposNomeados);
        }

        [HttpGet("totalizadores")]
        public async Task<JsonResult> ObterTotalizadores()
        {
            var quantidadeDePacientesVacinados = _repositorioPaciente.QuantidadeTotal();
            var quantidadeDeVacinasAplicadas = _repositorioDocumentoVacinacao.QuantidadeTotal();
            var quantidadeDeEstrangeiros = _repositorioPaciente.QuantidadeDe(P => P.NacionalidadeId == 2);
            var numEstabelecimentos = _repositorioEstabelecimento.QuantidadeTotal();


            var jsonData = new
            {
                quantidadeDePacientesVacinados,
                quantidadeDeVacinasAplicadas,
                quantidadeDeEstrangeiros,
                numEstabelecimentos,
                fabricanteComMaisDosesAplicadas = new
                {
                    fabricante = "PFIZER",
                    quantidade = 902040
                }
            };

            return new JsonResult(jsonData);

        }

    }
}
