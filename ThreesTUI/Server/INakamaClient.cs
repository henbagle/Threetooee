using Nakama;

namespace ThreesTUI.Server;

public interface INakamaClient
{
    IClient Client { get; }
    ISocket Socket { get; }
    ISession? Session { get; }
    Task<LoginResult> LogIn(string email, string password);
    Task LogOut();
}