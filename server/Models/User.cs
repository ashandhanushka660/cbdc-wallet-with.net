namespace server.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string NationalId { get; set; } = string.Empty;
    public double CreditScore { get; set; } = 0;
    public DateTime? LastScoredAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Wallet? Wallet { get; set; }
}
