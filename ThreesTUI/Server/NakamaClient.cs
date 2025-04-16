using System.Diagnostics;
using Nakama;

namespace ThreesTUI.Server;

public class NakamaClient : INakamaClient
{
    public IClient Client { get; }
    public ISocket Socket { get; }
    public ISession? Session { get; private set; }
    
    public NakamaClient()
    {
        Client = new Client("http", "10.52.1.15", 7350, "defaultkey");
        Client.Logger = new NakamaConsoleLogger();
        
        Socket = Nakama.Socket.From(Client);
        Socket.Connected += () =>
        {
            Console.WriteLine("Connected");
        };
        Socket.Closed += () =>
        {
            Console.WriteLine("Disconnected");
        };
        Socket.ReceivedError += e => Console.WriteLine(e);
    }

    public async Task<LoginResult> LogIn(string email, string password)
    {
        try
        {
            Session = await Client.AuthenticateEmailAsync(email, password, create: true);
            if (Session != null && Session.Created)
            {
                await Socket.ConnectAsync(Session, true);

            }
        }
        catch (ApiResponseException ex)
        {
            Debug.WriteLine($"Authentication error {ex.StatusCode}: {ex.Message}");
            return new LoginResult(false, ex.Message);
        }
        catch (Exception ex)
        {
            return new LoginResult(false, ex.Message);
        }
        
        return Session != null ? new LoginResult(true, "") : new LoginResult(false, $"Session not created.");
    }

    public async Task LogOut()
    {
        await Client.SessionLogoutAsync(Session);
    }
}