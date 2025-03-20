using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AshesMapBib.Models;
using Microsoft.Extensions.Logging;

public class GenericMultiApiService
{
    private readonly HttpClient _primaryClient;
    private readonly HttpClient _backupClient;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<GenericMultiApiService> _logger;

    public GenericMultiApiService(IHttpClientFactory clientFactory, ILogger<GenericMultiApiService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _primaryClient = clientFactory.CreateClient("PrimaryApi");
        _backupClient = clientFactory.CreateClient("BackupApi");
    }


    public async Task<List<NodePosition>> GetPrimaryDataAsync()
    {
        try
        {
            _logger.LogInformation("Abruf von PrimaryApi");
            return await _primaryClient.GetFromJsonAsync<List<NodePosition>>("api/resourcepositions_db");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen von PrimaryApi");
            throw;
        }
    }

    public async Task<List<NodePosition>> GetBackupDataAsync()
    {
        try
        {
            _logger.LogInformation("Abruf von BackupApi");
            return await _backupClient.GetFromJsonAsync<List<NodePosition>>("api/resourcepositions_db");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen von BackupApi");
            throw;
        }
    }
    /// <summary>
    /// Führt einen GET-Aufruf an den angegebenen Endpunkt durch und deserialisiert die Antwort in den Typ T.
    /// </summary>
    public async Task<T> GetAsync<T>(string clientName, string endpoint)
    {
        var client = _clientFactory.CreateClient(clientName);
        try
        {
            _logger.LogInformation("GET von {ClientName} an {Endpoint} wird aufgerufen.", clientName, endpoint);
            var result = await client.GetFromJsonAsync<T>(endpoint);
            _logger.LogInformation("GET erfolgreich: Daten empfangen.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim GET-Aufruf von {ClientName} an {Endpoint}.", clientName, endpoint);
            throw;
        }
    }

    /// <summary>
    /// Führt einen POST-Aufruf mit den übergebenen Daten durch und deserialisiert die Antwort in den Typ TResponse.
    /// </summary>
    public async Task<TResponse> PostAsync<TRequest, TResponse>(string clientName, string endpoint, TRequest data)
    {
        var client = _clientFactory.CreateClient(clientName);
        try
        {
            _logger.LogInformation("POST an {ClientName} an {Endpoint} mit Daten {Data} wird aufgerufen.", clientName, endpoint, data);
            var response = await client.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            _logger.LogInformation("POST erfolgreich: Antwort empfangen.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim POST-Aufruf an {ClientName} an {Endpoint} mit Daten {Data}.", clientName, endpoint, data);
            throw;
        }
    }

    /// <summary>
    /// Führt einen PUT-Aufruf mit den übergebenen Daten durch und deserialisiert die Antwort in den Typ TResponse.
    /// </summary>
    public async Task<TResponse> PutAsync<TRequest, TResponse>(string clientName, string endpoint, TRequest data)
    {
        var client = _clientFactory.CreateClient(clientName);
        try
        {
            _logger.LogInformation("PUT an {ClientName} an {Endpoint} mit Daten {Data} wird aufgerufen.", clientName, endpoint, data);
            var response = await client.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TResponse>();
            _logger.LogInformation("PUT erfolgreich: Antwort empfangen.");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim PUT-Aufruf an {ClientName} an {Endpoint} mit Daten {Data}.", clientName, endpoint, data);
            throw;
        }
    }

    /// <summary>
    /// Führt einen DELETE-Aufruf an den angegebenen Endpunkt durch.
    /// </summary>
    public async Task DeleteAsync(string clientName, string endpoint)
    {
        var client = _clientFactory.CreateClient(clientName);
        try
        {
            _logger.LogInformation("DELETE von {ClientName} an {Endpoint} wird aufgerufen.", clientName, endpoint);
            var response = await client.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("DELETE erfolgreich.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim DELETE-Aufruf an {ClientName} an {Endpoint}.", clientName, endpoint);
            throw;
        }
    }
}
