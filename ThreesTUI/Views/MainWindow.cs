using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using ThreesTUI.Server;

namespace ThreesTUI.Views;

public class MainWindow : Window
{
    private INakamaClient _client;
    public MainWindow(INakamaClient client)
    {
        _client = client;

        BorderStyle = LineStyle.None;

        _tabView = CreateTabView();
        _statusBar = CreateStatusBar();

        //LogIn();
        Add(_tabView, _statusBar);
    }

    public void LogIn()
    {
        // todo: check not already logged in
        var dlg = new LoginDialog(_client.LogIn);
        Application.Run(dlg);
        
        if (_loggedIn != null && _gameStatusMenuItem != null)
        {
            _gameStatusMenuItem.Title = $"Logged in as {_client.Session?.Username}.";
            _loggedIn.Title = "Log Out";
        }
        dlg.Dispose();
    }

    public async Task LogOut()
    {
        if (_loggedIn is null || _gameStatusMenuItem is null) return;
        
        if(_client.IsAuthenticated)
        {
            await _client.LogOut();
            _gameStatusMenuItem.Title = "Logged Out";
            _loggedIn.Title = "Log In";
        }
    }

    private readonly TabView? _tabView;

    private TabView CreateTabView()
    {
        var tabView = new TabView
        {
            Title = "Threes v0.1",
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
        };
        
        tabView.AddTab(new() { DisplayText = "Pong", View = Program.Services.GetRequiredService<PingView>()}, false);
        tabView.AddTab(new() { DisplayText = "Debug", View = Program.Services.GetRequiredService<DebugView>()}, false );

        tabView.SelectedTab = tabView.Tabs.First();
        return tabView;
    }
    
    private readonly StatusBar? _statusBar;
    private Shortcut? _loggedIn;
    private Shortcut? _gameStatusMenuItem;

    private StatusBar CreateStatusBar()
    {
        var statusBar = new StatusBar
        {
            CanFocus = false,
            AlignmentModes = AlignmentModes.IgnoreFirstOrLast
        };

        _loggedIn = new()
        {
            Title = "Log In",
            CanFocus = false,
            Key = Key.F10,
        };

        _loggedIn.Accepting += statusBarAccountAccepting;

        _gameStatusMenuItem = new()
        {
            Title = "Logged Out",
            CanFocus = false,
        };



        statusBar.Add(
            new Shortcut
            {
                CanFocus = false,
                Title = "Quit",
                Key = Application.QuitKey,
            },
            _loggedIn,
            _gameStatusMenuItem);
        return statusBar;
    }

    private async void statusBarAccountAccepting(object? sender, CommandEventArgs e)
    {
        Debug.Assert(_gameStatusMenuItem != null, nameof(_gameStatusMenuItem) + " != null");
        if (_gameStatusMenuItem.Title == "Logged Out")
        {
            LogIn();
        }
        else
        {
            await LogOut();
        }
    }
}