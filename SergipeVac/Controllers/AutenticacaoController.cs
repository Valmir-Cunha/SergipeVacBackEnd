using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model.Autenticacao;
using SergipeVac.Model.DTOs.Autenticacao;
using SergipeVac.Model.Interface;
using SergipeVac.Servicos;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IServicoToken _servicoToken;
        private readonly IRepositorio<Usuario> _repositorioUsuario;
        private readonly ServicoUsuario _servicoUsario;

        public AutenticacaoController(IServicoToken servicoToken, 
                                      IRepositorio<Usuario> repositorioUsuario, 
                                      ServicoUsuario servicoUsario)
        {
            _servicoToken = servicoToken;
            _repositorioUsuario = repositorioUsuario;
            _servicoUsario = servicoUsario;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioAuthDTO usuario)
        {
            var user = _repositorioUsuario.Obter(p => p.Email == usuario.Email).SingleOrDefault();

            if (user == null)
                return BadRequest("Usuário ou senha inválidos");

            //var senhaCriptografada = _servicoUsario.CriptografarSenha(usuario.Senha);

            if (!usuario.Senha.Equals(user.Senha))
            {
                return BadRequest("Usuário ou senha inválidos");
            }

            var token = _servicoToken.GerarToken(user);

            return new ContentResult
            {
                ContentType = "text/plain",
                Content = token,
                StatusCode = 200
            }; ;
        }
    }
}
