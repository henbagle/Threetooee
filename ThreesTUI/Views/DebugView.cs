using Terminal.Gui;
using ThreesTUI.Server;

namespace ThreesTUI.Views;

public class DebugView : TreeView
{
    private INakamaClient _client;
    public DebugView(INakamaClient client)
    {
        _client = client;
        
        Width = Dim.Fill();
        Height = Dim.Fill(1);
        
        AddObject(new TreeNode($"Host: {_client.Client.Host}"));
        AddObject(new TreeNode($"Port: {_client.Client.Port}"));
        AddObject(new TreeNode($"Socket Status: {(_client.Socket.IsConnected ? "Connected" : "Disconnected")}"));

        var session = new TreeNode("Session");
        if (_client.Session != null)
        {
            session.Children.Add(new TreeNode($"Session Uid: {_client.Session.UserId}"));
        }
        else
        {
            session.Children.Add(new TreeNode($"Not logged in."));
        }
        
        
        AddObject(session);
    }
    
    
}