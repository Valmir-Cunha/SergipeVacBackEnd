using Microsoft.AspNetCore.Mvc;
using SergipeVac.Conversores;

namespace SergipeVac.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ConversorDados _conversorDados;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ConversorDados conversorDados)
        {
            _logger = logger;
            _conversorDados = conversorDados;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public void Get()
        {
            _conversorDados.ConverterCSVDocumentosVacinacao();
        }
    }
}