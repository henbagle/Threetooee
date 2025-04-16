using Nakama;
using Terminal.Gui;
using ThreesTUI.Server;

namespace ThreesTUI.Views;

public class DebugView : View
{
    private INakamaClient _client;
    
    private TreeView _treeView;
    public DebugView(INakamaClient client)
    {
        _client = client;
        
        Width = Dim.Fill();
        Height = Dim.Fill(1);

        var btn = new Button()
        {
            Text = "Refresh",
            X = 0,
            Y = 0,
        };
        btn.Accepting += (a, e) => MakeTree();
        Add(btn);
        
        _treeView = new TreeView()
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        Add(_treeView);
        
        MakeTree();
    }

    public void MakeTree()
    {
        _treeView.ClearObjects();
        
        _treeView.AddObject(new TreeNode($"Host: {_client.Client.Host}:{_client.Client.Port}"));

        string socketStatus(ISocket socket)
        {
            if (socket.IsConnecting) return "Connecting...";
            return socket.IsConnected ? "Connected" : "Disconnected";
        }
        _treeView.AddObject(new TreeNode($"Socket Status: {socketStatus(_client.Socket)}"));

        var session = new TreeNode("Session");
        if (_client.Session != null)
        {
            session.Children.Add(new TreeNode($"Session Username: {_client.Session.Username}"));
            session.Children.Add(new TreeNode($"Session Uid: {_client.Session.UserId}"));
        }
        else
        {
            session.Children.Add(new TreeNode($"Not logged in."));
        }
        _treeView.AddObject(session);
    }

}