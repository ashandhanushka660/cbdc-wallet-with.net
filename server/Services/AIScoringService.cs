using System;

namespace server.Services;

/// <summary>
/// Implements the AI Scoring Architecture for rural CBDC borrowers.
/// Based on Logistic Regression + Sigmoid Probability of Default (PD).
/// </summary>
public class AIScoringService
{
    private const double Offset = 500; // Base score
    private const double Factor = 50;  // Points to double the odds (Scaling factor)
    
    // Weights (Beta coefficients) for the logistic regression model
    private readonly double _beta0 = -1.5; // Intercept
    private readonly double _wTelco = 0.8;
    private readonly double _wUtility = 1.2;
    private readonly double _wWallet = 1.5;
    private readonly double _wSocial = 0.5;

    public double CalculateScore(double xTelco, double xUtility, double xWallet, double xSocial)
    {
        // 1. Calculate the Feature Vector Sum
        double z = _beta0 + (_wTelco * xTelco) + (_wUtility * xUtility) + (_wWallet * xWallet) + (_wSocial * xSocial);

        // 2. Probability of Default (Sigmoid function)
        // PD = 1 / (1 + e^-z)
        double pd = 1.0 / (1.0 + Math.Exp(-z));

        // 3. Standardized Score (S)
        // S = Offset + Factor * ln((1-PD)/PD)
        // Note: ln((1-PD)/PD) is the 'logit' of (1-PD)
        double score = Offset + Factor * Math.Log((1.0 - pd) / pd);

        return Math.Round(score, 0);
    }
}
