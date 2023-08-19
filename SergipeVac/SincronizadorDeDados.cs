using SergipeVac.Servicos;

namespace SergipeVac
{
    public class SincronizadorDeDados
    {
        private readonly IServiceProvider _serviceProvider;

        public SincronizadorDeDados(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void SincronizarDados()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var servicoSincronizacao = serviceProvider.GetRequiredService<ServicoSincronizacao>();
                servicoSincronizacao.SincronizarDados();
            }
        }
    }
}
