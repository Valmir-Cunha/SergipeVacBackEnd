using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model.Autenticacao;
using SergipeVac.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepositorio<Usuario> _repositorioUsuario;
        public UsuarioController(IRepositorio<Usuario> repositorioUsuario)
        {
            _repositorioUsuario = repositorioUsuario;
        }

        [HttpPost("/cadastrar")]
        public Task<IActionResult> Cadastrar([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return Task.FromResult<IActionResult>(BadRequest(ModelState));
            }

            if (!new EmailAddressAttribute().IsValid(usuario.Email))
            {
                return Task.FromResult<IActionResult>(BadRequest("Email inválido"));
            }

            var usuarioExistente = _repositorioUsuario.Obter(u => u.Email == usuario.Email).SingleOrDefault();
            if (usuarioExistente != null)
            {
                return Task.FromResult<IActionResult>(Conflict("E-mail já cadastrado."));
            }

            try
            {
                _repositorioUsuario.Adicionar(usuario);
            }
            catch (Exception e)
            {
                return Task.FromResult<IActionResult>(BadRequest("Erro ao cadastrar usuário"));
            }

            return Task.FromResult<IActionResult>(Ok("Usuário cadastrado com sucesso!"));
        }

        [HttpPut("/editar/{codigo}")]
        public void Editar(int codigo)
        {
            // Lógica para editar o usuário com o código fornecido
            // ...
        }

        [HttpDelete("/excluir/{codigo}")]
        public Task<IActionResult> Excluir(int codigo)
        {
            var usuario = _repositorioUsuario.Obter(u => u.Codigo == codigo).SingleOrDefault();
            if (usuario == null)
            {
                return Task.FromResult<IActionResult>(BadRequest("Usuário não encontrado."));
            }

            try
            {
                _repositorioUsuario.Excluir(usuario);
            }
            catch (Exception e)
            {
                return Task.FromResult<IActionResult>(BadRequest("Erro ao excluir usuário"));
            }

            return Task.FromResult<IActionResult>(Ok("Usuário excluído com sucesso!"));
        }

        [HttpGet("/obterTodos")]
        public Task<IActionResult> ObterTodos()
        {
            var usuarios = _repositorioUsuario.ObterTodos();
            if (usuarios == null)
            {
                return Task.FromResult<IActionResult>(BadRequest("Usuários não encontrados."));
            }

            return Task.FromResult<IActionResult>(Ok(usuarios));
        }

        [HttpGet("/obter/{codigo}")]
        public Task<IActionResult> Obter(int codigo)
        {
            var usuario = _repositorioUsuario.Obter(u => u.Codigo == codigo).SingleOrDefault();
            if (usuario == null)
            {
                return Task.FromResult<IActionResult>(BadRequest("Usuário não encontrado."));
            }

            return Task.FromResult<IActionResult>(Ok(usuario));
        }
    }
}