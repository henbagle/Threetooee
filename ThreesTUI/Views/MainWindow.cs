using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;

namespace ThreesTUI.Views;

public class MainWindow : Window
{
    private readonly SessionManager _sessionManager;
    public MainWindow(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
        _sessionManager.AuthenticationChanged += OnAuthenticationChanged;

        BorderStyle = LineStyle.None;

        var tabView = CreateTabView();
        var statusBar = CreateStatusBar();
        
        Add(tabView, statusBar);
    }

    private void OnAuthenticationChanged(object? sender, bool isAuthenticated)
    {
        Application.Invoke(() =>
        {
            UpdateStatusBar(isAuthenticated);
        });
    }
    
    public void ShowLoginDialog()
    {
        // todo: check not already logged in
        var dlg = new LoginDialog(_sessionManager.LogIn);
        Application.Run(dlg);
        dlg.Dispose();
    }


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

    private Shortcut? _authenticateShortcut;
    private Shortcut? _gameStatusMenuItem;

    private StatusBar CreateStatusBar()
    {
        var statusBar = new StatusBar
        {
            CanFocus = false,
            AlignmentModes = AlignmentModes.IgnoreFirstOrLast
        };

        _authenticateShortcut = new()
        {
            Title = "Log In",
            CanFocus = false,
            Key = Key.F10,
        };

        _authenticateShortcut.Accepting += AuthenticateShortcutAccepting;

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
            _authenticateShortcut,
            _gameStatusMenuItem);
        return statusBar;
    }
    
    private void UpdateStatusBar(bool isAuthenticated)
    {
        if (_gameStatusMenuItem is null || _authenticateShortcut is null) return;
        if (isAuthenticated)
        {
            _gameStatusMenuItem.Title = $"Logged in as {_sessionManager.Username}";
            _authenticateShortcut.Title = "Log Out";
        }
        else
        {
            _gameStatusMenuItem.Title = "Logged Out";
            _authenticateShortcut.Title = "Log In";
        }
    }

    private void AuthenticateShortcutAccepting(object? sender, CommandEventArgs e)
    {
        Debug.Assert(_gameStatusMenuItem != null, nameof(_gameStatusMenuItem) + " != null");
        if (_gameStatusMenuItem.Title == "Logged Out") // TODO: check if logged in properly
        {
            ShowLoginDialog();
        }
        else
        {
            _sessionManager.LogOut().ConfigureAwait(false);
        }
    }
}