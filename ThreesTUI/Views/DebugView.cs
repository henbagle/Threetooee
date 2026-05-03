using System.Collections.Specialized;
using Nakama;
using Terminal.Gui;
using ThreesTUI.Logging;
using ThreesTUI.Server;

namespace ThreesTUI.Views;

public sealed class DebugView : View
{
    private readonly INakamaClient _client;
    private readonly SessionManager _sessionManager;
    private readonly TreeView _treeView;
    private readonly ListView _listView;
    public DebugView(INakamaClient client, SessionManager sessionManager, UILogSink uiLogSink)
    {
        _client = client;
        _sessionManager = sessionManager;
        _sessionManager.AuthenticationChanged += (_, _) => MakeTree();

        Width = Dim.Fill();
        Height = Dim.Fill();

        var top = new View()
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Percent(50)
        };
            
        var btn = new Button()
        {
            Text = "Refresh",
            X = 0,
            Y = 0,
        };
        btn.Accepting += (_, _) => MakeTree();
        top.Add(btn);
        
        _treeView = new TreeView()
        {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Percent(50) - 1,
        };
        top.Add(_treeView);

        _listView = new ListView()
        {
            X = 0,
            Y = Pos.Percent(50),
            Width = Dim.Fill(),
            Height = Dim.Percent(50),
            AllowsMarking = false,
            AllowsMultipleSelection = false
        };
        // TODO: Handle log entries wider than the screen and/or with newlines
        // TODO: Make a little pop up when you click on a log entry with the details - gets crazy with existing log sink
        _listView.SetSource(uiLogSink.Logs);
        
        Add(top);
        Add(_listView);
        
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