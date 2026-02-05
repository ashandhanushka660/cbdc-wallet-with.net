using System.Net.Http.Json;

namespace server.Services;

public class AIPredictionService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AIPredictionService> _logger;
    private readonly string _colabUrl;

    public AIPredictionService(HttpClient httpClient, ILogger<AIPredictionService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _colabUrl = Environment.GetEnvironmentVariable("COLAB_API_URL") ?? "";
    }

    public async Task<double?> GetCreditScoreAsync(string email, object features)
    {
        if (string.IsNullOrEmpty(_colabUrl))
        {
            _logger.LogWarning("COLAB_API_URL is not set. Falling back to local scoring.");
            return null;
        }

        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_colabUrl}/predict", features);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PredictionResult>();
                return result?.Score;
            }
            
            _logger.LogError("Colab API Error: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to AI Model in Colab.");
            return null;
        }
    }

    private class PredictionResult
    {
        public double Score { get; set; }
    }
}
