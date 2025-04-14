using Terminal.Gui;
using ThreesTUI.Server;

namespace ThreesTUI.Views;

public class PingView : View
{
    private INakamaClient _client;
    
    private Label _label;

    private int _count = 0;
    public PingView(INakamaClient client)
    {
        Width = Dim.Fill();
        Height = Dim.Fill();
        CanFocus = true;
        
        _client = client;
        var pingButton = new Button()
        {
            Text = "Ping",
            X = 0,
            Y = 0,
            Width = 30,
        };
        pingButton.Accepting += PingButtonOnAccepting;

        _label = new Label()
        {
            Text = "",
            X = Pos.Left(pingButton),
            Y = Pos.Bottom(pingButton) + 1,
            Width = 30,
        };
        
        Add(pingButton, _label);
    }

    private void PingButtonOnAccepting(object? sender, CommandEventArgs e)
    {
        _count++;
        if (_count < 2)
        {
            _label.Text = "Pong!";
        }
        else
        {
            _label.Text = $"Pong! (x{_count})";
        }
    }
}