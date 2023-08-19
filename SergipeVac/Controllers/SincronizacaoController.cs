using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model.Interface;
using SergipeVac.Model.Sincronizacao;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class SincronizacaoController : Controller
    {
        private readonly IRepositorio<DadosSincronizacao> _repositorioDadosSincronizacao;

        public SincronizacaoController(IRepositorio<DadosSincronizacao> repositorioDadosSincronizacao)
        {
            _repositorioDadosSincronizacao = repositorioDadosSincronizacao;
        }

        [HttpGet("obterUltimaSincronizacao")]
        public Task<IActionResult> ObterUltimaSincronizacao()
        {

            try
            {
                var sincronizacoes = _repositorioDadosSincronizacao.ObterTodos();
                var maiorData = sincronizacoes.Max(p => p.UltimaSincronizacao);
                var ultimaSincronizacao = sincronizacoes.Where(p => p.UltimaSincronizacao == maiorData);

                return Task.FromResult<IActionResult>(Ok(ultimaSincronizacao));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest("Nao foi obter última sincronização realizada."));
            }

        }

        [HttpGet("obterUltimaSincronizacaoBemSucedida")]
        public Task<IActionResult> ObterUltimaSincronizacaoBemSucedida()
        {

            try
            {
                var sincronizacoes = _repositorioDadosSincronizacao.Obter(p => p.BemSucedida);
                var maiorData = sincronizacoes.Max(p => p.UltimaSincronizacao);
                var ultimaSincronizacaoBemSucedida = sincronizacoes.Where(p => p.UltimaSincronizacao == maiorData);

                return Task.FromResult<IActionResult>(Ok(ultimaSincronizacaoBemSucedida));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest("Nao foi obter última sincronização bem-sucedida."));
            }

        }
    }
}