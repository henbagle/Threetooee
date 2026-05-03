using Microsoft.Extensions.Logging;
using Nakama;
using ThreesTUI.Logging;

namespace ThreesTUI.Server;

public class NakamaClient : INakamaClient
{
    public IClient Client { get; }
    public ISocket Socket { get; }
    public ISession? Session { get; private set; }
    
    public bool IsAuthenticated => Session is { IsExpired: false, IsRefreshExpired: false };
    
    private ILogger<NakamaClient> _logger;
    
    public NakamaClient(NakamaSerilogAdapter logAdapter, ILogger<NakamaClient> logger)
    {
        Client = new Client("http", "10.52.1.15", 7350, "defaultkey");
        Client.Logger = logAdapter;
        _logger = logger;
        
        Socket = Nakama.Socket.From(Client);
        Socket.ReceivedError += Console.WriteLine;
    }

    public async Task<LoginResult> LogIn(string email, string password)
    {
        try
        {
            Session = await Client.AuthenticateEmailAsync(email, password, create: true);
            if (IsAuthenticated)
            {
                await Socket.ConnectAsync(Session, true);

            }
        }
        catch (ApiResponseException ex)
        {
            _logger.LogError(ex, "Login error: {Message}", ex.Message);
            return new LoginResult(false, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error: {Message}", ex.Message);
            return new LoginResult(false, ex.Message);
        }
        
        return Session != null ? new LoginResult(true, "") : new LoginResult(false, $"Session not created.");
    }

    public async Task LogOut()
    {
        await Client.SessionLogoutAsync(Session);
        Session = null;
        if (Socket.IsConnected)
        {
            await Socket.CloseAsync();
        }
    }
}