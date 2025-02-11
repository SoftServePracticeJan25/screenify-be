using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DebugController> _logger;

    public DebugController(IConfiguration configuration, ILogger<DebugController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet("secrets")]
    public IActionResult GetAllSecrets()
    {
        try
        {
            _logger.LogInformation("Запрос на получение всех секретов.");

            // Получаем все конфигурационные данные (appsettings.json + user-secrets + env vars)
            var secrets = _configuration.AsEnumerable()
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return Ok(secrets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении секрета");
            return StatusCode(500, new { Error = "Ошибка сервера", Details = ex.Message });
        }
    }

}
