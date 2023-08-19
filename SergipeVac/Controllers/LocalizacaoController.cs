using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model.Interface;
using SergipeVac.Model.Localizacao;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class LocalizacaoController : Controller
    {

        private readonly IRepositorio<Localizacao> _repositoriolocalizacao;
        public LocalizacaoController(IRepositorio<Localizacao> repositoriolocalizacao)
        {
            _repositoriolocalizacao = repositoriolocalizacao;
        }

        [HttpPost("adicionarlocal")]
        public Task<IActionResult> AdicionarLocal([FromBody] Localizacao localizacao)
        {
            if (!ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            var cidadeExistente = _repositoriolocalizacao.Obter(u => u.Cidade == localizacao.Cidade).SingleOrDefault();

            if (cidadeExistente != null)
            {
                try
                {
                    cidadeExistente.Quantidade += 1;

                    _repositoriolocalizacao.Atualizar(cidadeExistente);

                    return Task.FromResult<IActionResult>(Ok("adicinada localizacao."));
                }
                catch (Exception ex)
                {
                    return Task.FromResult<IActionResult>(BadRequest("não foi possivel adicionar."));
                }
            }

            try
            {
                _repositoriolocalizacao.Adicionar(localizacao);

                return Task.FromResult<IActionResult>(Ok("Nova cidade catalogada."));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest("Nao foi possivel adicionar uma nova cidade"));
            }

        }

        [HttpGet("obterlocais")]
        public Task<IActionResult> ObterLocais()
        {
            try
            {
                var locais = _repositoriolocalizacao.ObterTodos();

                return Task.FromResult<IActionResult>(Ok(locais));

            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest("erro em obter locais."));
            }
        }
    }
}