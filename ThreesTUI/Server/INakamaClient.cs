using Nakama;

namespace ThreesTUI.Server;

public interface INakamaClient
{
    IClient Client { get; }
    ISocket Socket { get; }
    ISession? Session { get; }
    Task<bool> LogIn(string email, string password);
    void LogOut();
}