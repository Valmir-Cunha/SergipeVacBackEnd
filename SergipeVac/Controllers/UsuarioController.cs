using Microsoft.AspNetCore.Mvc;
using SergipeVac.Model.Autenticacao;
using SergipeVac.Model.Interface;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController
    {
        private readonly IRepositorio<Usuario> _repositorioUsuario;
        public UsuarioController(IRepositorio<Usuario> repositorioUsuario)
        {
            _repositorioUsuario=repositorioUsuario;
        }

        [HttpPost("/usuarios")]
        public void Cadastrar() { }

        [HttpPut("/usuarios/{codigo}")]
        public void Editar(int codigo) { }

        [HttpDelete("/usuarios/{codigo}")]
        public void Excluir(int codigo) { }

        [HttpGet("/usuarios")]
        public void ObterTodos() { }

        [HttpGet("/usuarios/{codigo}")]
        public void Obter(int codigo) { }
    }
}
