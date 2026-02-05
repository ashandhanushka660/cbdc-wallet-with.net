using server.Models;
using server.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace server.Services;

/// <summary>
/// The 'Networker' Service: A background worker that links the .NET application 
/// to the Google Colab AI model for real-time credit scoring updates.
/// NOW UPDATED TO USE REST API FOR STABILITY.
/// </summary>
public class CreditScoringWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CreditScoringWorker> _logger;
    private readonly string _supabaseUrl = "https://ucxdhrikpgxgzbdkwzfc.supabase.co/rest/v1";
    private readonly string _supabaseKey = "sb_publishable_LzMUxGAt_JjwnANm2kupuA_ocSQnIDi";

    public CreditScoringWorker(IServiceProvider serviceProvider, ILogger<CreditScoringWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Networker Service (REST-mode) started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("apikey", _supabaseKey);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _supabaseKey);

                // 1. Find users who haven't been scored yet via REST
                var userResponse = await client.GetAsync($"{_supabaseUrl}/Users?LastScoredAt=is.null&limit=5", stoppingToken);
                
                if (userResponse.IsSuccessStatusCode)
                {
                    var pendingUsers = await userResponse.Content.ReadFromJsonAsync<List<User>>(stoppingToken);

                    if (pendingUsers != null && pendingUsers.Count > 0)
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var aiPredictor = scope.ServiceProvider.GetRequiredService<AIPredictionService>();
                        var localAiService = scope.ServiceProvider.GetRequiredService<AIScoringService>();

                        foreach (var user in pendingUsers)
                        {
                            _logger.LogInformation("Processing AI Credit Score for: {Email}", user.Email);

                            // Prepare Features
                            var features = new { 
                                telco = 0.8, 
                                utility = 0.7, 
                                wallet = 0.9, 
                                social = 0.5 
                            };

                            // Get score (Colab or Fallback)
                            double? score = await aiPredictor.GetCreditScoreAsync(user.Email, features);
                            if (score == null)
                            {
                                score = localAiService.CalculateScore(0.8, 0.7, 0.9, 0.5);
                            }

                            // 2. Update User via REST PATCH
                            var updateData = new {
                                CreditScore = score.Value,
                                LastScoredAt = DateTime.UtcNow
                            };

                            var patchResponse = await client.PatchAsJsonAsync($"{_supabaseUrl}/Users?Id=eq.{user.Id}", updateData, stoppingToken);
                            
                            if (patchResponse.IsSuccessStatusCode)
                                _logger.LogInformation("Successfully updated score ({Score}) for {Email}", score.Value, user.Email);
                            else
                                _logger.LogError("Failed to update user score via REST: {Code}", patchResponse.StatusCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Networker Service.");
            }

            // Sync interval
            await Task.Delay(TimeSpan.FromSeconds(45), stoppingToken);
        }
    }
}
