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
    
    private readonly double _beta0 = -1.5; // Intercept
    private readonly double _wTelco = 0.8;
    private readonly double _wUtility = 1.2;
    private readonly double _wWallet = 1.5;
    private readonly double _wSocial = 0.5;

    // Recovery Index Constants (Equation 4)
    private const double RiBase = 100.0;
    private const double Lambda = 0.05; // Rehabilitation decay factor

    public double CalculateScore(double xTelco, double xUtility, double xWallet, double xSocial)
    {
        // 1. Calculate the Feature Vector Sum (Equation 1)
        double z = _beta0 + (_wTelco * xTelco) + (_wUtility * xUtility) + (_wWallet * xWallet) + (_wSocial * xSocial);

        // 2. Probability of Default (Sigmoid function - Equation 2)
        double pd = 1.0 / (1.0 + Math.Exp(-z));

        // 3. Standardized Score (S - Equation 3)
        double score = Offset + Factor * Math.Log((1.0 - pd) / pd);

        return Math.Round(score, 0);
    }

    /// <summary>
    /// Recovery and Rehabilitation Algorithm (RI) - Equation 4
    /// </summary>
    /// <param name="daysSinceDefault">Delta t</param>
    /// <param name="recoveryContributions">Sum of gamma_j * Delta C_j (Payment behavior)</param>
    public double CalculateRecoveryIndex(int daysSinceDefault, double recoveryContributions)
    {
        // RI(t) = RIbase * (1 - e^-lambda*dt) + sum(gamma * dC)
        double timeFactor = RiBase * (1.0 - Math.Exp(-Lambda * daysSinceDefault));
        double ri = timeFactor + recoveryContributions;

        return Math.Round(ri, 2);
    }
}
