using Microsoft.Extensions.Logging;
using ThreesTUI.Server;

namespace ThreesTUI;

public class SessionManager(INakamaClient client, ILogger<SessionManager> logger)
{
    public event EventHandler<bool>? AuthenticationChanged;

    public bool IsAuthenticated => client.IsAuthenticated;
    public string? Username => client.Session?.Username;
    
    public async Task<LoginResult> LogIn(string email, string password)
    {
        var result = await client.LogIn(email, password);
        if (result.Success)
        {
            AuthenticationChanged?.Invoke(this, true);
        }
        return result;
    }
    
    public async Task LogOut()
    {
        if(IsAuthenticated)
        {
            try
            {
                await client.LogOut();
                AuthenticationChanged?.Invoke(this, false);
            }
            catch (Exception ex)
            {
                logger.LogError("Error logging out: {message}", ex.Message);
            }
        }
    }
}