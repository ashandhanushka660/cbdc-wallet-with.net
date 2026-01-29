namespace server.Models;

public class Wallet
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string WalletAddress { get; set; } = string.Empty;
    public decimal Balance { get; set; } = 0;
    public string Currency { get; set; } = "CBDC";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public User User { get; set; } = null!;
}
